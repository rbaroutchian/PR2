using System.Data;
using Microsoft.Data.SqlClient;

namespace PlaceReserv.Data
{
    public class DatabaseContext
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public DatabaseContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("sqlconnection");
        }

       

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}

