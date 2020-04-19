using GeekTime.Infrastructure.Core;
using GeekTime.Ordering.Domain.OrderAggregate;

namespace GeekTime.Ordering.Infrastructure.Repositories
{
    public interface IOrderRepository : IRepository<Order, long>
    {

    }
}
