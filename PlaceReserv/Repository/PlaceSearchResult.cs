using PlaceReserv.Models;
using PlaceReserv.Repository;

namespace PlaceReserv.Repository
{
    public  class PlaceSearchResult
    {
        public int TotalCount { get; set; }
        public required IEnumerable<Place> Places { get; set; }
    }
}