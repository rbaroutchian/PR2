using PlaceReserv.Data;
using PlaceReserv.Models;
using Dapper;
using System.Security.AccessControl;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PlaceReserv.Interfaces;


namespace PlaceReserv.Repository
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly DatabaseContext _dbContext;

        public PlaceRepository(DatabaseContext dbContext) => _dbContext = dbContext;
        

        public async Task<Place> GetPlaceByIdAsync([FromQuery]int id)
        {
            var query = "SELECT * FROM Places WHERE Id = @Id";
            using (var connection = _dbContext.CreateConnection())
            {
                var place=await connection.QueryFirstOrDefaultAsync<Place>(query, new { Id = id });
                return place;
            }
        }

        public async Task<IEnumerable<Place>> GetAllPlacesAsync()
        {
            var query = "SELECT * FROM Places";
            using (var connection = _dbContext.CreateConnection())
            {
                
                
                var Place=await connection.QueryAsync<Place>(query);
                return Place.ToList();
            }
        }

        public async Task AddPlaceAsync(Place place)
        {
            var query = "INSERT INTO Places (Title, Address, PlaceType, Location, RegistrationDate, RegistrantId) VALUES (@Title, @Address, @PlaceType, @Location, @RegistrationDate, @RegistrantId)";
            using (var connection = _dbContext.CreateConnection())
            {
                
                await connection.ExecuteAsync(query, place);
            }
        }

        public async Task UpdatePlaceAsync(Place place)
        {
            var query = "UPDATE Places SET Title = @Title, Address = @Address, PlaceType = @PlaceType, Location = @Location WHERE Id = @Id";
            using (var connection = _dbContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, place);
            }
        }

        public async Task DeletePlaceAsync(int id)
        {
            var query = "DELETE FROM Places WHERE Id = @Id";
            using (var connection = _dbContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        // using procedures
        public async Task<PlaceSearchResult> SearchPlacesAsync(PlaceSearchCriteria criteria)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Title", criteria.Title, DbType.String, size: 100, direction: ParameterDirection.Input);
                parameters.Add("@PlaceType", criteria.PlaceType, DbType.String, size: 50, direction: ParameterDirection.Input);
                parameters.Add("@PageSize", criteria.PageSize, DbType.Int32, direction: ParameterDirection.Input);
                parameters.Add("@PageNumber", criteria.PageNumber, DbType.Int32, direction: ParameterDirection.Input);
                parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                try
                {
                    var results = await connection.QueryAsync<Place>("SearchPlaces", parameters, commandType: CommandType.StoredProcedure);
                    var totalCount = parameters.Get<int>("TotalCount");

                    return new PlaceSearchResult
                    {
                        TotalCount = totalCount,
                        Places = results
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }


        
        }
    }
}
        
        








        


 
    


        

    



