using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly;
namespace GrpcClientDemo.Interceptors
{
    public class PollyInterceptor : Interceptor
    {
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var policy = Policy.Handle<RpcException>().Retry(3);
            return policy.Execute(() => base.AsyncUnaryCall(request, context, continuation));
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var policy = Policy.Handle<RpcException>().Retry(3);
            return policy.Execute(() => base.BlockingUnaryCall(request, context, continuation));
        }
    }
}
