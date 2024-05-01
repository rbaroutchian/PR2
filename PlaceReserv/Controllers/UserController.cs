using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaceReserv.Models;
using JWT.Builder;
using PlaceReserv.Controllers;
using Microsoft.AspNetCore.Identity;
using PlaceReserv.Interfaces;

namespace PlaceReserv.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            
        }
        
        [HttpGet("{id}")]
        
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await userRepository.GetAllUsersAsync();
            return Ok(users);
        }
        
        [HttpPost]

        public async Task<ActionResult<User>> CreateUser(User user)
        {
            await userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                await userRepository.UpdateUserAsync(user);
            }
            catch (Exception)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await userRepository.DeleteUserAsync(id);

            return NoContent();
        }

        private bool UserExists(int id)
        {
            var user = userRepository.GetUserByIdAsync(id);
            return user != null;
        }
    }
}

