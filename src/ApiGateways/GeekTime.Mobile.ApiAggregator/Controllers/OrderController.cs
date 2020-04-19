using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekTime.Mobile.ApiAggregator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekTime.Mobile.ApiAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IOrderService _orderService;
        GeekTime.Ordering.API.Grpc.OrderService.OrderServiceClient _client;
        public OrderController(IOrderService orderService, GeekTime.Ordering.API.Grpc.OrderService.OrderServiceClient client)
        {
            _orderService = orderService;
            _client = client;

        }

        [HttpGet]
        public ActionResult GetOrders([FromQuery]Ordering.API.Grpc.SearchRequest request)
        {
            //添加其它服务的调用;
            var data = _client.Search(request);
            return Ok(data.Orders);
        }
    }
}