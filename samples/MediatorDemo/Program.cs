using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorDemo
{
    class Program
    {
        async static Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddMediatR(typeof(Program).Assembly);

            var serviceProvider = services.BuildServiceProvider();

            var mediator = serviceProvider.GetService<IMediator>();


            await mediator.Publish(new MyEvent { EventName = "event01" });
            //await mediator.Send(new MyCommand { CommandName = "cmd01" });
            Console.WriteLine("Hello World!");
        }
    }
    #region 
    internal class MyCommand : IRequest<long>
    { 
        public string CommandName { get; set; }
    }

    

    internal class MyCommandHandler : IRequestHandler<MyCommand, long>
    {
        public Task<long> Handle(MyCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"MyCommandHandler执行命令：{request.CommandName}");
            return Task.FromResult(10L);
        }
    }
    #endregion
    #region
    internal class MyEvent : INotification
    { 
        public string EventName { get; set; }
    }

    
    //
    internal class MyEventHandler : INotificationHandler<MyEvent>
    {
        public Task Handle(MyEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"MyEventHandler执行：{notification.EventName}");
            return Task.CompletedTask;
        }
    }

    internal class MyEventHandlerV2 : INotificationHandler<MyEvent>
    {
        public Task Handle(MyEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"MyEventHandlerV2执行：{notification.EventName}");
            return Task.CompletedTask;
        }
    }

    #endregion
}
