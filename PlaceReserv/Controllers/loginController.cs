using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using PlaceReserv.Models;
using PlaceReserv.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using PlaceReserv.Controllers;


namespace PlaceReserv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        


        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
           
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Login login)
        {
            // احراز هویت
            bool isAuthenticated = _loginService.Authenticate(login.Username, login.Password);

            if (isAuthenticated)
            {
                // تولید توکن
                string token = _loginService.GenerateToken(login.Username);

                

                // بازگشت توکن به عنوان نتیجه
                return Ok(new { token = token });
            }
            else
            {
                // احراز هویت ناموفق بوده است
                return Unauthorized("Authentication failed");
            }
        }
    }
}







