using Grpc.Core;
using GrpcServices;
using System.Threading.Tasks;

namespace GrpcServerDemo.GrpcServices
{
    public class OrderService : OrderGrpc.OrderGrpcBase
    {
        public override Task<CreateOrderResult> CreateOrder(CreateOrderCommand request, ServerCallContext context)
        {

            throw new System.Exception("order error");

            //添加创建订单的内部逻辑，录入将订单信息存储到数据库
            return Task.FromResult(new CreateOrderResult { OrderId = 24 });
        }
    }
}
