using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
namespace GeekTime.Ordering.API.Application.Queries
{
    public class MyOrderQuery : IRequest<List<string>>
    {
        public string UserName { get; set; }
    }
}
