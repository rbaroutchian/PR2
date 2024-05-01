using PlaceReserv.Models;

namespace PlaceReserv.Services
{
    public interface IReservationService
    {
        Task<Reservation> GetReservationByIdAsync(int id, int userId, string username, string password);
        Task AddReservationAsync(Reservation reservation, int userId, string username, string password);
        Task<IEnumerable<Reservation>> GetAllReservationsAsync(int userId, string username, string password);
        Task UpdateReservationAsync(Reservation reservation, int userId, string username, string password);
        Task DeleteReservationAsync(int id, int userId, string username, string password);
        Task<List<Reservation>> GetReservationsByUserAsync(int userId, string username, string password);
    }
}

