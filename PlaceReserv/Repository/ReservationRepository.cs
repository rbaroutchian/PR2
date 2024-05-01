using PlaceReserv.Data;
using PlaceReserv.Models;
using System.ComponentModel.Design;
using Dapper;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PlaceReserv.Interfaces;

namespace PlaceReserv.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly DatabaseContext _dbContext;

        public ReservationRepository(DatabaseContext dbContext) => _dbContext = dbContext;


        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            var query = "SELECT * FROM Reservations WHERE Id = @Id";
            using (var connection = _dbContext.CreateConnection())
            {
                var reserv = await connection.QueryFirstOrDefaultAsync<Reservation>(query, new { Id = id });
                return reserv;
            }
        }
        //check for duplicate reservation
        public async Task AddReservationAsync(Reservation reservation)
        {
            var query1 = "SELECT * FROM Reservations WHERE ReservationDate = @ReservationDate AND PlaceId = @PlaceId";
            var query2 = "INSERT INTO Reservations (RegistrationDate, ReservationDate, UserId, PlaceId, Amount) " +
                "VALUES (@RegistrationDate, @ReservationDate, @UserId, @PlaceId, @Amount)";
            using (var connection = _dbContext.CreateConnection())
            {
                //Duplicate booking check 
                var existingReservation = await connection.QueryFirstOrDefaultAsync<Reservation>(query1,
                 new { ReservationDate = reservation.ReservationDate, PlaceId = reservation.PlaceId });

                if (existingReservation != null)
                {
                    throw new InvalidOperationException("This place is already booked on this date");
                }

                await connection.ExecuteAsync(query2, reservation);

            }
        }


        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            var query = "SELECT * FROM Reservations";
            using (var connection = _dbContext.CreateConnection())
            {
                return await connection.QueryAsync<Reservation>(query);
            }
        }




        public async Task UpdateReservationAsync(Reservation reservation)
        {
            var query = "UPDATE Reservations SET RegistrationDate = @RegistrationDate, ReservationDate = @ReservationDate, UserId = @UserId, PlaceId = @PlaceId, Amount = @Amount WHERE Id = @Id";
            using (var connection = _dbContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, reservation);
            }
        }

        public async Task DeleteReservationAsync(int id)
        {
            var query = "DELETE FROM Reservations WHERE Id = @Id";
            using (var connection = _dbContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task<List<Reservation>> GetReservationsByUserAsync(int userId)
        {

            var query = "SELECT * FROM Reservations WHERE UserId = @UserId";
            using (var connection = _dbContext.CreateConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<Reservation>(query, userId);
                if (user == null)
                {
                    return new List<Reservation>();
                }
                return new List<Reservation> { user };
            }
        }
    }
}


