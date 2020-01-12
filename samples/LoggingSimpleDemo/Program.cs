using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
namespace LoggingSimpleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = configBuilder.Build();
            
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfiguration>(p => config); //用工厂模式将配置对象注册到容器管理

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConfiguration(config.GetSection("Logging"));
                builder.AddConsole();
            });


            serviceCollection.AddTransient<OrderService>();

            IServiceProvider service = serviceCollection.BuildServiceProvider();



            var order = service.GetService<OrderService>();

            order.Show();

            ILoggerFactory loggerFactory = service.GetService<ILoggerFactory>();

            //ILogger alogger = loggerFactory.CreateLogger("alogger");

            //alogger.LogDebug(2001, "aiya");
            //alogger.LogInformation("hello");

            //var ex = new Exception("出错了");
            //alogger.LogError(ex, "出错了");

            //var alogger2 = loggerFactory.CreateLogger("alogger");

            //alogger2.LogDebug("aiya");


            

            //var logger = service.GetService<ILogger<Program>>();

            //logger.LogInformation(new EventId(201, "xihuaa"), "hello world!");

            Console.ReadKey();
        }
    }
}
