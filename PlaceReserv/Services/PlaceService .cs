using PlaceReserv.Interfaces;
using PlaceReserv.Models;

namespace PlaceReserv.Services
{
    public class PlaceService:IPlaceService
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly ILoginService _loginService;

        public PlaceService(IPlaceRepository placeRepository, ILoginService loginService)
        {
            _placeRepository = placeRepository;
            _loginService = loginService;
        }

        public async Task<Place> GetPlaceByIdAsync(int id, string username, string password)
        {
            if (!_loginService.Authenticate(username, password))
            {
                throw new UnauthorizedAccessException("Authentication failed.");
            }

            var place = await _placeRepository.GetPlaceByIdAsync(id);
            if (place == null)
            {
                return null;
            }

            // You might want to implement additional authorization logic here based on your requirements

            return place;
        }

        public async Task<IEnumerable<Place>> GetAllPlacesAsync(string username, string password)
        {
            if (!_loginService.Authenticate(username, password))
            {
                throw new UnauthorizedAccessException("Authentication failed.");
            }

            // You might want to implement access control here based on your requirements

            return await _placeRepository.GetAllPlacesAsync();
        }

        public async Task AddPlaceAsync(Place place, string username, string password)
        {
            if (!_loginService.Authenticate(username, password))
            {
                throw new UnauthorizedAccessException("Authentication failed.");
            }

            await _placeRepository.AddPlaceAsync(place);
        }

        public async Task UpdatePlaceAsync(Place place, string username, string password)
        {
            if (!_loginService.Authenticate(username, password))
            {
                throw new UnauthorizedAccessException("Authentication failed.");
            }

            await _placeRepository.UpdatePlaceAsync(place);
        }

        public async Task DeletePlaceAsync(int id, string username, string password)
        {
            if (!_loginService.Authenticate(username, password))
            {
                throw new UnauthorizedAccessException("Authentication failed.");
            }

            await _placeRepository.DeletePlaceAsync(id);
        }

        // You can add additional methods here based on your requirements
    }
}

