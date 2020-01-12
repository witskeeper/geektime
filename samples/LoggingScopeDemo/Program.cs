using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace LoggingScopeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddCommandLine(args);
            configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = configBuilder.Build();
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(p => config); //用工厂模式将配置对象注册到容器管理
            serviceCollection.AddLogging(builder =>
            {
                builder.AddConfiguration(config.GetSection("Logging"));
                builder.AddConsole();
                builder.AddDebug();
            });

            IServiceProvider service = serviceCollection.BuildServiceProvider();

            var logger = service.GetService<ILogger<Program>>();


            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                using (logger.BeginScope("ScopeId:{scopeId}", Guid.NewGuid()))
                {
                    logger.LogInformation("这是Info");
                    logger.LogError("这是Error");
                    logger.LogTrace("这是Trace");
                }
                System.Threading.Thread.Sleep(100);
                Console.WriteLine("============分割线=============");
            }
            Console.ReadKey();
        }
    }
}
