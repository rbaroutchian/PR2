using PlaceReserv.Data;
using PlaceReserv.Models;
using Dapper;
using PlaceReserv.Repository;
using Microsoft.SqlServer;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PlaceReserv.Interfaces;

namespace PlaceReserv.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly DatabaseContext _dbContext;

        public UserRepository(DatabaseContext dbContext)=>_dbContext = dbContext;
        

        public async Task<User> GetUserByIdAsync([FromQuery]int id)
        {
            var query = "SELECT * FROM Users WHERE Id = @Id";
            using (var connection = _dbContext.CreateConnection())
            {
                var user= await connection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
                return user;
            }
        }

        public async Task<User> GetUserByUsernameAsync([FromQuery]string username)
        {
            var query = "SELECT * FROM Users WHERE Username = @Username";
            using (var connection = _dbContext.CreateConnection())
            {
                var user= await connection.QueryFirstOrDefaultAsync<User>(query, new { Username = username });
                return user;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var query = "SELECT * FROM Users";
            using (var connection = _dbContext.CreateConnection())
            {
                return await connection.QueryAsync<User>(query);
            }
        }

        public async Task AddUserAsync(User user)
        {
            var query = "INSERT INTO Users (Username, Password, IsActive, RegistrationDate) VALUES (@Username, @Password, @IsActive, @RegistrationDate)";
            using (var connection = _dbContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, user);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            var query = "UPDATE Users SET Username = @Username, Password = @Password, IsActive = @IsActive WHERE Id = @Id";
            using (var connection = _dbContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, user);
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var query = "DELETE FROM Users WHERE Id = @Id";
            using (var connection = _dbContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}

