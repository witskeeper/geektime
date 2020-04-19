using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Registry;

namespace GrpcClientDemo.Extensions
{
    public static class PollyServiceCollectionExtensions
    {
        public static void AddPollyClient(this IServiceCollection services)
        {
            var r = services.AddPolicyRegistry();
            r.Add("timeout", Policy.Timeout(5));
            r.Add("retry", Polly.Extensions.Http.HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(3, t => TimeSpan.FromSeconds(t + 1)));

            r.Add("circuitBreaker", HttpPolicyExtensions.HandleTransientHttpError()
                .AdvancedCircuitBreakerAsync(failureThreshold: 0.5,
                samplingDuration: TimeSpan.FromSeconds(5),
                minimumThroughput: 100,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (ex, state, ts, context) => { },
                onReset: context => { },
                onHalfOpen: () => { }));

            services.AddHttpClient("pollyclient").AddPolicyHandlerFromRegistry("timeout");
            services.AddHttpClient("smartClient").AddPolicyHandlerFromRegistry((registry, message) =>
            {
                return HttpMethod.Get.Equals(message.Method) ? registry.Get<IAsyncPolicy<HttpResponseMessage>>("retry") : Policy.NoOpAsync<HttpResponseMessage>();
            });


            //为每个请求注册一个熔断器策略，单独计算其状态
            services.AddHttpClient("circuitBreaker").AddPolicyHandler((provider, message) =>
            {
                var registry = provider.GetService<IConcurrentPolicyRegistry<string>>(); //这里需要Polly 7.2+ 
                var key = $"circuitBreaker_{message.RequestUri.AbsolutePath}";
                return registry.GetOrAdd(key, Policy.HandleResult<HttpResponseMessage>(response =>
                {
                    return response.StatusCode == HttpStatusCode.RequestTimeout;
                }) //这里应该捕获非网络相关的异常，而不是全局异常，如果是全局网络异常，则断路器应该针对所有请求
                 .AdvancedCircuitBreakerAsync(failureThreshold: 0.5,
                 samplingDuration: TimeSpan.FromSeconds(5),
                 minimumThroughput: 100,
                 durationOfBreak: TimeSpan.FromSeconds(20),
                 onBreak: (ex, state, ts, context) => { },
                 onReset: context => { },
                 onHalfOpen: () => { }));
            });


            services.AddHttpClient("wrapClient").AddPolicyHandler(PolicyWrap()); //添加组合重试+降级+断路器

        }


        static void GetRetryPolicy()
        {
            //重试3次
            Policy.Handle<RpcException>().Retry(retryCount: 3, onRetry: (ex, t, context) => { });

            //重试直到成功
            Policy.Handle<RpcException>().RetryForever(onRetry: (ex, t, context) => { });

            Policy.Handle<RpcException>()
                  .WaitAndRetry(new[]
                  {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4),
                    TimeSpan.FromSeconds(8),
                    TimeSpan.FromSeconds(15),
                    TimeSpan.FromSeconds(30)
                  });

            Policy.Handle<RpcException>()
                  .WaitAndRetry(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                  );

            //等待并重试
            Policy.Handle<RpcException>().WaitAndRetry(retryCount: 10, sleepDurationProvider: t => TimeSpan.FromSeconds(t), onRetry: (ex, ts, t, context) => { });

            //等待并重试直到成功
            Policy.Handle<RpcException>().WaitAndRetryForever(sleepDurationProvider: (t, context, ts) => TimeSpan.FromSeconds(t * 2), onRetry: (ex, ts, t, context) => { });
        }

        static void GetTimeOutPolicy()
        {
            Policy.Timeout(20);

            Policy.Timeout(20, onTimeout: (context, ts, task) => { });

            //悲观超时,不取消超时任务
            Policy.Timeout(20, Polly.Timeout.TimeoutStrategy.Pessimistic);

        }


        static void GetCircuitBreakerPolicy()
        {
            //出现3次异常则熔断
            Policy.Handle<RpcException>().CircuitBreaker(exceptionsAllowedBeforeBreaking: 3, durationOfBreak: TimeSpan.FromSeconds(20));

            //出现3次异常则熔断
            Policy.Handle<RpcException>().CircuitBreaker(exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (ex, state, ts, context) => { },
                onReset: context => { },
                onHalfOpen: () => { });


            //高级熔断
            Policy.Handle<RpcException>().AdvancedCircuitBreaker(failureThreshold: 0.5,
                samplingDuration: TimeSpan.FromSeconds(5),
                minimumThroughput: 100,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (ex, state, ts, context) => { },
                onReset: context => { },
                onHalfOpen: () => { });

        }


        static void GetBulkheadPolicy()
        {
            
             var bulkhead =  Policy.Bulkhead(maxParallelization: 100, maxQueuingActions: 20, onBulkheadRejected: context => { });
            bulkhead.Execute(() => { });
        }


        static IAsyncPolicy<HttpResponseMessage> PolicyWrap()
        {
            /*
            fallback.Execute(() => waitAndRetry.Execute(() => breaker.Execute(action)));
            fallback.Wrap(waitAndRetry).Wrap(breaker).Execute(action);
            fallback.Wrap(waitAndRetry.Wrap(breaker)).Execute(action);
            Policy.Wrap(fallback, waitAndRetry, breaker).Execute(action);
            */

            HttpResponseMessage defaultMessage = new HttpResponseMessage(HttpStatusCode.OK);
            defaultMessage.Content = new StringContent("{}");
            var bcFallback = Policy<HttpResponseMessage>.Handle<BrokenCircuitException>().FallbackAsync<HttpResponseMessage>(defaultMessage);

            var retry = Policy<HttpResponseMessage>.Handle<Exception>().WaitAndRetryAsync(3, t => TimeSpan.FromSeconds(t + 1));

            var bc = Policy<HttpResponseMessage>.Handle<RpcException>().AdvancedCircuitBreakerAsync(failureThreshold: 0.5,
                samplingDuration: TimeSpan.FromSeconds(5),
                minimumThroughput: 100,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (ex, state, ts, context) => { },
                onReset: context => { },
                onHalfOpen: () => { });

            return Policy.WrapAsync(retry, bcFallback, bc);
        }
    }
}
