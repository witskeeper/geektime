using GeekTime.Domain.OrderAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Domain.Events
{
    public class OrderCreatedDomainEvent : IDomainEvent
    {
        public Order Order { get; private set; }
        public OrderCreatedDomainEvent(Order order)
        {
            this.Order = order;
        }
    }
}
