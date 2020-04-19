using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekTime.Ordering.API.Application.IntegrationEvents
{
    public interface ISubscriberService
    {
        void OrderPaymentSucceeded(OrderPaymentSucceededIntegrationEvent @event);
    }
}
