using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
namespace GeekTime.Mobile.Gateway.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public IActionResult Abc()
        {
            return Content("GeekTime.Mobile.Gateway");
        }

        public IActionResult ShowConfig([FromServices]IConfiguration configuration)
        {
            return Content(configuration["ENV_ABC"]);
        }


        [HttpGet]
        public IActionResult ShowRequestUri()
        {
            return Content(Request.GetDisplayUrl());
        }

        [HttpGet]
        public IActionResult ShowHeaders()
        {
            var sb = new System.Text.StringBuilder();
            foreach (var item in Request.Headers)
            {
                sb.AppendLine($"{item.Key}:{item.Value}");
            }

            return Content(sb.ToString());
        }
    }
}