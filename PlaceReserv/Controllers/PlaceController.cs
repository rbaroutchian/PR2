using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaceReserv.IRepository;
using PlaceReserv.Models;


namespace PlaceReserv.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceRepository placeRepository;
        
        public PlaceController(IPlaceRepository placeRepository)//, UserManager<AuthService> userManager)//, SignInManager<AuthService> signInManager)
        {
            this.placeRepository = placeRepository;
            //this.userManager = userManager;
            //this.signInManager = signInManager;
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Place>> GetPlace(int id)
        {
            var place = await placeRepository.GetPlaceByIdAsync(id);
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
            var places = await placeRepository.GetAllPlacesAsync();
            return Ok(places);
        }
        [HttpPost]
        [Authorize]
        
        public async Task<ActionResult<Place>> CreatePlace(Place place)
        {
            
            await placeRepository.AddPlaceAsync(place);
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
                await placeRepository.UpdatePlaceAsync(place);
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
            var place = await placeRepository.GetPlaceByIdAsync(id);
            if (place == null)
            {
                return NotFound();
            }

            await placeRepository.DeletePlaceAsync(id);

            return NoContent();
        }

        private bool PlaceExists(int id)
        {
            var place = placeRepository.GetPlaceByIdAsync(id);
            return place != null;
        }
    }
}

