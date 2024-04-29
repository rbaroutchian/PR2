using PlaceReserv.Data;
using PlaceReserv.IRepository;
using PlaceReserv.Models;
using System.ComponentModel.Design;
using Dapper;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace PlaceReserv.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly DatabaseContext dbContext;

        public ReservationRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<Reservation>("SELECT * FROM Reservations WHERE Id = @Id", new { Id = id });
            }
        }
        public async Task AddReservationAsync(Reservation reservation)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                //Duplicate booking check 
                var existingReservation = await connection.QueryFirstOrDefaultAsync<Reservation>("SELECT * FROM Reservations WHERE ReservationDate = @ReservationDate AND PlaceId = @PlaceId",
                 new { ReservationDate = reservation.ReservationDate, PlaceId = reservation.PlaceId });

                if (existingReservation != null)
                {
                    throw new InvalidOperationException("This place is already booked on this date");
                }
                var sql = "INSERT INTO Reservations (RegistrationDate, ReservationDate, UserId, PlaceId, Amount) " +
                "VALUES (@RegistrationDate, @ReservationDate, @UserId, @PlaceId, @Amount)";
                await connection.ExecuteAsync(sql, reservation);

            }
        }


        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                return await connection.QueryAsync<Reservation>("SELECT * FROM Reservations");
            }
        }




        public async Task UpdateReservationAsync(Reservation reservation)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync("UPDATE Reservations SET RegistrationDate = @RegistrationDate, ReservationDate = @ReservationDate, UserId = @UserId, PlaceId = @PlaceId, Amount = @Amount WHERE Id = @Id", reservation);
            }
        }

        public async Task DeleteReservationAsync(int id)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync("DELETE FROM Reservations WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<List<Reservation>> GetReservationsByUserAsync(string userId)
        {

            var query = "SELECT * FROM Reservations WHERE UserId = @UserId";
            using (var connection = dbContext.CreateConnection())
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


