using PlaceReserv.Models;

namespace PlaceReserv.Interfaces
{
    public interface IPlaceRepository
    {
        public Task<Place> GetPlaceByIdAsync(int id);
        public Task<IEnumerable<Place>> GetAllPlacesAsync();
        public Task AddPlaceAsync(Place place);
        public Task UpdatePlaceAsync(Place place);
        public Task DeletePlaceAsync(int id);
        public Task<PlaceSearchResult> SearchPlacesAsync(PlaceSearchCriteria criteria);


    }
}

