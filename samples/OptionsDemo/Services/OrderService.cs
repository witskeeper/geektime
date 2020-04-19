using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
namespace OptionsDemo.Services
{
    public interface IOrderService
    {
        int ShowMaxOrderCount();
    }
    public class OrderService : IOrderService
    {
        IOptions<OrderServiceOptions> _options;
        public OrderService(IOptions<OrderServiceOptions> options)
        {
            _options = options;


            //_options.OnChange(option =>
            //{
            //    Console.WriteLine($"配置更新了，最新的值是:{_options.CurrentValue.MaxOrderCount}");
            //});
        }

        public int ShowMaxOrderCount()
        {
            return _options.Value.MaxOrderCount;
        }
    }

    public class OrderServiceOptions
    {
        [Range(30, 100)]
        public int MaxOrderCount { get; set; } = 100;
    }


    public class OrderServiceValidateOptions : IValidateOptions<OrderServiceOptions>
    {
        public ValidateOptionsResult Validate(string name, OrderServiceOptions options)
        {
            if (options.MaxOrderCount > 100)
            {
                return ValidateOptionsResult.Fail("MaxOrderCount 不能大于100");
            }
            else
            {
                return ValidateOptionsResult.Success;
            }
        }
    }

}

