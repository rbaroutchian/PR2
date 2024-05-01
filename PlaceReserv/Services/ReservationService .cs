using PlaceReserv.Interfaces;
using PlaceReserv.Models;

namespace PlaceReserv.Services
{
    public class ReservationService:IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ILoginService _loginService;



        public ReservationService(IReservationRepository reservationRepository, ILoginService loginService)
        {
            _reservationRepository = reservationRepository;
            _loginService = loginService;


        }

        public async Task<Reservation> GetReservationByIdAsync(int id, int userId, string username, string password)
        {
            if (!_loginService.Authenticate(username,password))
            {
                throw new UnauthorizedAccessException("You are not authenticated.");
            }
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return null;
            }

            // Check if the reservation belongs to the authenticated user
            if (reservation.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to access this reservation.");
            }

            return reservation;
        }

        public async Task AddReservationAsync(Reservation reservation, int userId,string username, string password)
        {
            if (!_loginService.Authenticate(username, password))
            {
                throw new UnauthorizedAccessException("Authentication failed.");
            }
            reservation.UserId = userId;
            await _reservationRepository.AddReservationAsync(reservation);
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync(int userId, string username, string password)
        {
            if (!_loginService.Authenticate(username, password))
            {
                throw new UnauthorizedAccessException("Authentication failed.");
            }
            // You might want to implement access control here based on your requirements
            return await _reservationRepository.GetAllReservationsAsync();
        }

        public async Task UpdateReservationAsync(Reservation reservation, int userId, string username, string password)
        {
            if (!_loginService.Authenticate(username, password))
            {
                throw new UnauthorizedAccessException("Authentication failed.");
            }
            var existingReservation = await GetReservationByIdAsync(reservation.Id, userId, username, password);
            if (existingReservation == null)
            {
                throw new InvalidOperationException("Reservation not found.");
            }

            // Ensure that the reservation being updated belongs to the authenticated user
            reservation.UserId = userId;
            await _reservationRepository.UpdateReservationAsync(reservation);
        }

        public async Task DeleteReservationAsync(int id, int userId, string username, string password)
        {
            if (!_loginService.Authenticate(username, password))
            {
                throw new UnauthorizedAccessException("Authentication failed.");
            }
            var existingReservation = await GetReservationByIdAsync(id, userId, username, password);
            if (existingReservation == null)
            {
                throw new InvalidOperationException("Reservation not found.");
            }

            // Ensure that the reservation being deleted belongs to the authenticated user
            await _reservationRepository.DeleteReservationAsync(id);
        }

        public async Task<List<Reservation>> GetReservationsByUserAsync(int userId, string username, string password)
        {
            if (!_loginService.Authenticate(username, password))
            {
                throw new UnauthorizedAccessException("Authentication failed.");
            }
            // You might want to implement access control here based on your requirements
            return await _reservationRepository.GetReservationsByUserAsync(userId);
        }
    }
}