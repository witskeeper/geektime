using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OptionsDemo.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OrderServiceExtensions
    {
        public static IServiceCollection AddOrderService(this IServiceCollection services,IConfiguration configuration)
        {

            //services.Configure<OrderServiceOptions>(configuration);

            #region 后期更新
            //services.PostConfigure<OrderServiceOptions>(options =>
            //{
            //    options.MaxOrderCount += 20;
            //});
            #endregion
            #region 添加验证

            //services.AddOptions<OrderServiceOptions>().Configure(options =>
            //{
            //    configuration.Bind(options);
            //}).Validate(options => options.MaxOrderCount > 100);

            //services.AddOptions<OrderServiceOptions>().Configure(options =>
            //{
            //    configuration.Bind(options);
            //}).ValidateDataAnnotations();

            


            services.AddOptions<OrderServiceOptions>().Configure(options =>
            {
                configuration.Bind(options);
            }).Services.AddSingleton<IValidateOptions<OrderServiceOptions>>(new OrderServiceValidateOptions( ));


            #endregion


            services.AddSingleton<IOrderService, OrderService>();
            return services;
        }

        public static IServiceCollection AddOrderService(this IServiceCollection services, Action<OrderServiceOptions> setup)
        {
            services.Configure<OrderServiceOptions>(setup);
            services.AddScoped<IOrderService, OrderService>();
            return services;
        }


        public static IServiceCollection AddOrderServiceWithV(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OrderServiceOptions>(configuration);
            services.AddSingleton<IOrderService, OrderService>();
            return services;
        }
    }
}
