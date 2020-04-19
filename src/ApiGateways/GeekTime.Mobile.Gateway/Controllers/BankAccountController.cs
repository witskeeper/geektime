using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GeekTime.Mobile.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BankAccountController : Controller
    {
        [Authorize]
        public IActionResult Cookie()
        {
            return Content("bank account");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Jwt()
        {
            return Content(User.FindFirst("Name").Value);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult AnyOne()
        {
            return Content(User.FindFirst("Name").Value);
        }
    }
}