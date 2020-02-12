using System;
using System.Collections.Generic;
using System.Text;
using GeekTime.Domain.OrderAggregate;
using GeekTime.Infrastructure.Core;
namespace GeekTime.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order, long, DomainContext>, IOrderRepository
    {
        public OrderRepository(DomainContext context) : base(context)
        {
        }
    }
}
