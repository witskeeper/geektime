using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
namespace GeekTime.Domain
{
    public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent> 
        where TDomainEvent : IDomainEvent
    {
        //这里我们使用了INotificationHandler的Handle方法来作为处理方法的定义
        //Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
