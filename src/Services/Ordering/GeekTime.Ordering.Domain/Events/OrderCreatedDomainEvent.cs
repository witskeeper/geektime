using GeekTime.Domain;
using GeekTime.Ordering.Domain.OrderAggregate;

namespace GeekTime.Ordering.Domain.Events
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
