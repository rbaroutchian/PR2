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

namespace PlaceReserv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IConfiguration configuration, IRegistrationService registrationService)
        {
            _configuration = configuration;
            _registrationService = registrationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(User user)
        {
            bool isRegistered = _registrationService.Register(user);
            if (isRegistered)
            {
                return Ok("Registration successful.");
            }
            else
            {
                return BadRequest("Registration failed.");
            }
        }
    }
}







