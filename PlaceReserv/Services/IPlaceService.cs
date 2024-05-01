using PlaceReserv.Models;

namespace PlaceReserv.Services
{
    public interface IPlaceService
    {
        Task<Place> GetPlaceByIdAsync(int id, string username, string password);
        Task<IEnumerable<Place>> GetAllPlacesAsync(string username, string password);
        Task AddPlaceAsync(Place place, string username, string password);
        Task UpdatePlaceAsync(Place place, string username, string password);
        Task DeletePlaceAsync(int id, string username, string password);
    
    }
}
