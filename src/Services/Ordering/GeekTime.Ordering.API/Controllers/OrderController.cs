using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GeekTime.Ordering.API.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using GeekTime.Ordering.API.Application.Queries;

namespace GeekTime.Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<long> CreateOrder([FromBody]CreateOrderCommand cmd)
        {
            return await _mediator.Send(cmd, HttpContext.RequestAborted);
        }



        [HttpGet]
        public async Task<List<string>> QueryOrder([FromQuery]MyOrderQuery myOrderQuery)
        {
            return await _mediator.Send(myOrderQuery);
        }

        #region 不建议的写法
        //[HttpPost]
        //public Task<long> CreateOrder([FromBody]CreateOrderVeiwModel viewModel)
        //{
        //    var model = viewModel.ToModel();
        //    return await orderService.CreateOrder(model);
        //}


        //class OrderService:IOrderService
        //{
        //    public long CreateOrder(CreateOrderModel model)
        //    {
        //        var address = new Address("wen san lu", "hangzhou", "310000");
        //        var order = new Order("xiaohong1999", "xiaohong", 25, address);

        //        _orderRepository.Add(order);
        //        await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        //        return order.Id;
        //    }
        //}
        #endregion
    }
}