namespace PlaceReserv.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ReservationDate { get; set; }
        public int UserId { get; set; }
        public int PlaceId { get; set; }
        public decimal Amount { get; set; }
    }
}

