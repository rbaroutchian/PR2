using PlaceReserv.Data;
using PlaceReserv.Models;
using Dapper;
using PlaceReserv.Repository;
using PlaceReserv.IRepository;
using Microsoft.SqlServer;
using System.Data.SqlTypes;

namespace PlaceReserv.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly DatabaseContext dbContext;

        public UserRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Username = @Username", new { Username = username });
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                return await connection.QueryAsync<User>("SELECT * FROM Users");
            }
        }

        public async Task AddUserAsync(User user)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync("INSERT INTO Users (Username, Password, IsActive, RegistrationDate) VALUES (@Username, @Password, @IsActive, @RegistrationDate)", user);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync("UPDATE Users SET Username = @Username, Password = @Password, IsActive = @IsActive WHERE Id = @Id", user);
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync("DELETE FROM Users WHERE Id = @Id", new { Id = id });
            }
        }
    }
}

