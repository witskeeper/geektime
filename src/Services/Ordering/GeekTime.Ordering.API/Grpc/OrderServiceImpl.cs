using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekTime.Ordering.API.Grpc
{
    public class OrderServiceImpl : OrderService.OrderServiceBase
    {
        IMediator _mediator;
        ILogger _logger;

        public OrderServiceImpl(IMediator mediator, ILogger<OrderServiceImpl> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public override async Task<Int64Value> CreateOrder(CreateOrderCommand request, ServerCallContext context)
        {
            var cmd = new GeekTime.Ordering.API.Application.Commands.CreateOrderCommand(request.ItemCount);
            var r = await _mediator.Send(cmd);
            return new Int64Value { Value = r };
        }

        public override async Task<SearchResponse> Search(SearchRequest request, ServerCallContext context)
        {
            var query = new GeekTime.Ordering.API.Application.Queries.MyOrderQuery { UserName = request.UserName };
            var r = await _mediator.Send(query);
            var response = new SearchResponse();
            response.Orders.AddRange(r);
            return response;
        }
    }
}
