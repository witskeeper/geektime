using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DependencyInjectionScopeAndDisposableDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DependencyInjectionScopeAndDisposableDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public int Get(
            [FromServices]IHostApplicationLifetime hostApplicationLifetime, 
            [FromQuery]bool stop = false)
        {


            #region 
            //Console.WriteLine("=======1==========");
            //using (IServiceScope scope = HttpContext.RequestServices.CreateScope())
            //{
            //    var service = scope.ServiceProvider.GetService<IOrderService>();
            //}
            //Console.WriteLine("=======2==========");
            #endregion

            #region
            if (stop)
            {
               hostApplicationLifetime.StopApplication();
            }
            #endregion

            //Console.WriteLine("接口请求处理结束");
            return 1;
        }
    }
}
