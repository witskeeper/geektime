using GeekTime.Domain.UserAggregate;
using GeekTime.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Infrastructure.Repositories
{
    class UserRepository : Repository<User, long, DomainContext>
    {
        public UserRepository(DomainContext context) : base(context)
        {
        }
    }
}
