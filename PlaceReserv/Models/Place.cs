namespace PlaceReserv.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public string? PlaceType { get; set; }
        public string? Location { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int RegisteredById { get; set; }
    }
}
        
