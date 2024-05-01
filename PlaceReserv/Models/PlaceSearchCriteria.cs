namespace PlaceReserv.Models
{
    public class PlaceSearchCriteria
    {
        public string Title { get; set; }
        public string PlaceType { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}

