namespace PlaceReserv.Models
{
    public class PlaceSearchResult
    {
        public int TotalCount { get; set; }
        public required IEnumerable<Place> Places { get; set; }
    }
}