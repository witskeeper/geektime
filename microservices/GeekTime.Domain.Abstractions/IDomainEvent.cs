using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
namespace GeekTime.Domain
{
    public interface IDomainEvent : INotification
    {
    }
}
