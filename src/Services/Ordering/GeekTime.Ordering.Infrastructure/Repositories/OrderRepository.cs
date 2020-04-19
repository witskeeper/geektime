using GeekTime.Infrastructure.Core;
using GeekTime.Ordering.Domain.OrderAggregate;

namespace GeekTime.Ordering.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order, long, OrderingContext>, IOrderRepository
    {
        public OrderRepository(OrderingContext context) : base(context)
        {
        }
    }
}
