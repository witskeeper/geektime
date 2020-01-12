using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiddlewareDemo.Middlewares;
namespace Microsoft.AspNetCore.Builder
{
    public static class MyBuilderExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyMiddleware>();
        }
    }
}
