using GeekTime.Domain;
using GeekTime.Domain.OrderAggregate;
using GeekTime.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Infrastructure.Repositories
{
    public interface IOrderRepository : IRepository<Order, long>
    {

    }
}
