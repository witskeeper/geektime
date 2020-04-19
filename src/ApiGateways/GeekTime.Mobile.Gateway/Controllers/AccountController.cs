using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GeekTime.Mobile.Gateway.Controllers
{
    [ApiController]
    [Route("account/[action]")]
    public class AccountController : Controller
    {

        public async Task<string> Login()
        {
            return await Task.FromResult("请先登录");
        }


        public async Task<IActionResult> CookieLogin(string userName)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);//一定要声明AuthenticationScheme
            identity.AddClaim(new Claim("Name", userName));
            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            return Content("login");
        }


        public async Task<IActionResult> JwtLogin([FromServices]SymmetricSecurityKey securityKey, string userName)
        {

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Name", userName));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            var t = new JwtSecurityTokenHandler().WriteToken(token);
            return Content(t);
        }



    }
}