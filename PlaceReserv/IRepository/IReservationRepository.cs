using PlaceReserv.Models;

namespace PlaceReserv.IRepository
{
    public interface IReservationRepository
    {
        public Task<Reservation> GetReservationByIdAsync(int id);
        public Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        public Task AddReservationAsync(Reservation reservation);
        public Task UpdateReservationAsync(Reservation reservation);
        public Task DeleteReservationAsync(int id);
        public Task<List<Reservation>> GetReservationsByUserAsync(string userId);

    }
}

