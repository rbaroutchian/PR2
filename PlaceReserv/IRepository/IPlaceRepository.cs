using PlaceReserv.Models;
using PlaceReserv.Repository;

namespace PlaceReserv.IRepository
{
    public interface IPlaceRepository
    {
        public Task<Place> GetPlaceByIdAsync(int id);
        public Task<IEnumerable<Place>> GetAllPlacesAsync();
        public  Task AddPlaceAsync(Place place);
        public Task UpdatePlaceAsync(Place place);
        public Task DeletePlaceAsync(int id);
        public Task<PlaceSearchResult> SearchPlacesAsync(string title = null, string placeType = null, int pageSize = 10, int pageNumber = 1);


    }
}

