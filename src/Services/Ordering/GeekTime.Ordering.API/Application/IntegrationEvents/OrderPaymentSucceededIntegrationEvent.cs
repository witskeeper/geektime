namespace GeekTime.Ordering.API.Application.IntegrationEvents
{
    public class OrderPaymentSucceededIntegrationEvent
    {
        public OrderPaymentSucceededIntegrationEvent(long orderId) => OrderId = orderId;
        public long OrderId { get; }
    }

}
