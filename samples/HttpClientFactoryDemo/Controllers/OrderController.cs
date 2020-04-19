using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpClientFactoryDemo.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientFactoryDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        OrderServiceClient _orderServiceClient;
        public OrderController(OrderServiceClient orderServiceClient)
        {
            _orderServiceClient = orderServiceClient;
        }

        [HttpGet("Get")]
        public async Task<string> Get()
        {
            return await _orderServiceClient.Get();
        }

        [HttpGet("NamedGet")]
        public async Task<string> NamedGet([FromServices]NamedOrderServiceClient serviceClient)
        {
            return await serviceClient.Get();
        }

        [HttpGet("TypedGet")]
        public async Task<string> TypedGet([FromServices]TypedOrderServiceClient serviceClient)
        {
            return await serviceClient.Get();
        }
    }
}