using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
namespace LoggingSimpleDemo
{
    public class OrderService
    {
        ILogger<OrderService> _logger;
        public OrderService(ILogger<OrderService> logger)
        {
            _logger = logger;
        }

        public void Show()
        {
            _logger.LogInformation("Show Time{time}", DateTime.Now);

           

            _logger.LogInformation($"Show Time{DateTime.Now}");
        }
    }
}
