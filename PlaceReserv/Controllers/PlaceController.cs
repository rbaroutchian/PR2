using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaceReserv.Interfaces;
using PlaceReserv.Models;
using PlaceReserv.Repository;
using PlaceReserv.Services;


namespace PlaceReserv.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly ILoginService _loginService;

        public PlaceController(IPlaceRepository placeRepository, ILoginService loginService)
        {
            _placeRepository = placeRepository;
            _loginService = loginService;
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Place>> GetPlace(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                return Unauthorized("Token not found");
            }

            var username = _loginService.ValidateToken(token);
            if (username == null)
            {
                return Unauthorized("Invalid token");
            }

            var place = await _placeRepository.GetPlaceByIdAsync(id);
            if (place == null)
            {
                return NotFound();
            }
            return place;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces()
        {
            var places = await _placeRepository.GetAllPlacesAsync();
            return Ok(places);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Place>> CreatePlace(Place place)
        {
            await _placeRepository.AddPlaceAsync(place);
            return CreatedAtAction(nameof(GetPlace), new { id = place.Id }, place);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePlace(int id, Place place)
        {
            if (id != place.Id)
            {
                return BadRequest();
            }

            try
            {
                await _placeRepository.UpdatePlaceAsync(place);
            }
            catch (Exception)
            {
                if (!PlaceExists(id))
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
        [Authorize]
        public async Task<IActionResult> DeletePlace(int id)
        {
            var place = await _placeRepository.GetPlaceByIdAsync(id);
            if (place == null)
            {
                return NotFound();
            }

            await _placeRepository.DeletePlaceAsync(id);

            return NoContent();
        }

        private bool PlaceExists(int id)
        {
            var place = _placeRepository.GetPlaceByIdAsync(id);
            return place != null;
        }
    }
}