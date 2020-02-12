using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
namespace GeekTime.API.Application.Queries
{
    public class MyOrderQuery : IRequest<List<string>>
    {
        public MyOrderQuery(string userName) => UserName = userName;

        public string UserName { get; private set; }
    }
}
