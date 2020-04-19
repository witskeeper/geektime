using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
namespace GeekTime.Ordering.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Abc()
        {
            return Content("GeekTime.Ordering.API");
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


        [HttpGet]
        public IActionResult Error()
        {
            throw new Exception("这是个模拟异常");
        }
    }
}