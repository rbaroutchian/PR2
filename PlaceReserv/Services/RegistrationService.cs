using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using PlaceReserv.Services;
using PlaceReserv.Controllers;
using PlaceReserv.Models;


namespace PlaceReserv.Services
{


    public class RegistrationService : IRegistrationService
    {
        private readonly IConfiguration _configuration;

        public RegistrationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Register(User user)
        {
            var connectionString = _configuration.GetConnectionString("sqlconnection");

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var query = "INSERT INTO Users (Username, Password, IsActive, RegistrationDate) VALUES (@Username, @Password, @IsActive, @RegistrationDate)";
                    var affectedRows = connection.Execute(query, user);

                    // If at least one row is affected, registration is successful
                    return affectedRows > 0;
                }
                catch (SqlException)
                {
                    // Handle exception if necessary
                    return false;
                }
            }
        }
    }
}