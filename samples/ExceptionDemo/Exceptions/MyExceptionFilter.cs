using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace ExceptionDemo.Exceptions
{
    public class MyExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            IKnownException knownException = context.Exception as IKnownException;
            if (knownException == null)
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<MyExceptionFilterAttribute>>();
                logger.LogError(context.Exception, context.Exception.Message);
                knownException = KnownException.Unknown;
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            else
            {
                knownException = KnownException.FromKnownException(knownException);
                context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            }
            context.Result = new JsonResult(knownException)
            {
                ContentType = "application/json; charset=utf-8"
            };
        }
    }
}
