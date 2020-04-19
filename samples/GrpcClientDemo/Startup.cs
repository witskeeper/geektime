using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GrpcClientDemo.Interceptors;
using GrpcServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Microsoft.Extensions.Http;
using Polly.Extensions.Http;
using static GrpcServices.OrderGrpc;
using Polly.CircuitBreaker;
using System.Net;
using Polly.Bulkhead;
namespace GrpcClientDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true); //允许使用不加密的HTTP/2协议
            services.AddGrpcClient<OrderGrpc.OrderGrpcClient>(options =>
            {
                options.Address = new Uri("https://localhost:5001");
            })
            .ConfigurePrimaryHttpMessageHandler(provider =>
            {
                var handler = new SocketsHttpHandler();
                handler.SslOptions.RemoteCertificateValidationCallback = (a, b, c, d) => true; //允许无效、或自签名证书
                return handler;
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryForeverAsync(i => TimeSpan.FromSeconds(i * 3)));


            var reg = services.AddPolicyRegistry();

            reg.Add("retryforever", Policy.HandleResult<HttpResponseMessage>(message =>
            {
                return message.StatusCode == System.Net.HttpStatusCode.Created;
            }).RetryForeverAsync());




            services.AddHttpClient("orderclient").AddPolicyHandlerFromRegistry("retryforever");
            services.AddHttpClient("orderclientv2").AddPolicyHandlerFromRegistry((registry, message) =>
            {
                return message.Method == HttpMethod.Get ? registry.Get<IAsyncPolicy<HttpResponseMessage>>("retryforever") : Policy.NoOpAsync<HttpResponseMessage>();
            });

            services.AddHttpClient("orderclientv3").AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>().CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 10,
                durationOfBreak: TimeSpan.FromSeconds(10),
                onBreak: (r, t) => { },
                onReset: () => { },
                onHalfOpen: () => { }
                ));


            services.AddHttpClient("orderclientv3").AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>().AdvancedCircuitBreakerAsync(
                failureThreshold: 0.8,
                samplingDuration: TimeSpan.FromSeconds(10),
                minimumThroughput: 100,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (r, t) => { },
                onReset: () => { },
                onHalfOpen: () => { }));


            var breakPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>().AdvancedCircuitBreakerAsync(
                failureThreshold: 0.8,
                samplingDuration: TimeSpan.FromSeconds(10),
                minimumThroughput: 100,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (r, t) => { },
                onReset: () => { },
                onHalfOpen: () => { });

            var message = new HttpResponseMessage()
            {
                Content = new StringContent("{}")
            };
            var fallback = Policy<HttpResponseMessage>.Handle<BrokenCircuitException>().FallbackAsync(message);
            var retry = Policy<HttpResponseMessage>.Handle<Exception>().WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(1));
            var fallbackBreak = Policy.WrapAsync(fallback, retry, breakPolicy);
            services.AddHttpClient("httpv3").AddPolicyHandler(fallbackBreak);


            var bulk = Policy.BulkheadAsync<HttpResponseMessage>(
                maxParallelization: 30,
                maxQueuingActions: 20,
                onBulkheadRejectedAsync: contxt => Task.CompletedTask
                );

            var message2 = new HttpResponseMessage()
            {
                Content = new StringContent("{}")
            };
            var fallback2 = Policy<HttpResponseMessage>.Handle<BulkheadRejectedException>().FallbackAsync(message);
            var fallbackbulk = Policy.WrapAsync(fallback2, bulk);
            services.AddHttpClient("httpv4").AddPolicyHandler(fallbackbulk);


            #region

            //reg.Add("circuitBreaker", HttpPolicyExtensions.HandleTransientHttpError()
            //    .AdvancedCircuitBreakerAsync(failureThreshold: 0.5,
            //    samplingDuration: TimeSpan.FromSeconds(5),
            //    minimumThroughput: 100,
            //    durationOfBreak: TimeSpan.FromSeconds(20),
            //    onBreak: (ex, state, ts, context) => { },
            //    onReset: context => { },
            //    onHalfOpen: () => { }));


            //HttpResponseMessage defaultMessage = new HttpResponseMessage(HttpStatusCode.OK);
            //defaultMessage.Content = new StringContent("{}");
            //var bcFallback = Policy<HttpResponseMessage>.Handle<BrokenCircuitException>().FallbackAsync<HttpResponseMessage>(defaultMessage);

            //var retry = Policy<HttpResponseMessage>.Handle<Exception>().WaitAndRetryAsync(3, t => TimeSpan.FromSeconds(t + 1));

            //var bc = Policy<HttpResponseMessage>.Handle<HttpRequestException>().AdvancedCircuitBreakerAsync(failureThreshold: 0.5,
            //    samplingDuration: TimeSpan.FromSeconds(5),
            //    minimumThroughput: 100,
            //    durationOfBreak: TimeSpan.FromSeconds(20),
            //    onBreak: (ex, state, ts, context) => { },
            //    onReset: context => { },
            //    onHalfOpen: () => { });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    OrderGrpcClient service = context.RequestServices.GetService<OrderGrpcClient>();

                    try
                    {
                        var r = service.CreateOrder(new CreateOrderCommand { BuyerId = "abc" });
                    }
                    catch (Exception ex)
                    {
                    }

                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
