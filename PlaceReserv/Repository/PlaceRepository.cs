using PlaceReserv.Data;
using PlaceReserv.IRepository;
using PlaceReserv.Models;
using Dapper;
using System.Security.AccessControl;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace PlaceReserv.Repository
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly DatabaseContext dbContext;

        public PlaceRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Place> GetPlaceByIdAsync(int id)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<Place>("SELECT * FROM Places WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<IEnumerable<Place>> GetAllPlacesAsync()
        {
            var query = "SELECT * FROM Places";
            using (var connection = dbContext.CreateConnection())
            {
                
                
                var Place=await connection.QueryAsync<Place>(query);
                return Place.ToList();
            }
        }

        public async Task AddPlaceAsync(Place place)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync("INSERT INTO Places (Title, Address, PlaceType, Location, RegistrationDate, RegistrantId) VALUES (@Title, @Address, @PlaceType, @Location, @RegistrationDate, @RegistrantId)", place);
            }
        }

        public async Task UpdatePlaceAsync(Place place)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync("UPDATE Places SET Title = @Title, Address = @Address, PlaceType = @PlaceType, Location = @Location WHERE Id = @Id", place);
            }
        }

        public async Task DeletePlaceAsync(int id)
        {
            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync("DELETE FROM Places WHERE Id = @Id", new { Id = id });
            }
        }

        // using procedures
         async Task<PlaceSearchResult> IPlaceRepository.SearchPlacesAsync(string title, string placeType, int pageSize, int pageNumber)
        {
            

            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Title", title, DbType.String, size: 100, direction: ParameterDirection.Input);
                parameters.Add("@PlaceType", placeType, DbType.String, size: 50, direction: ParameterDirection.Input);
                parameters.Add("@PageSize", pageSize, DbType.Int32, direction: ParameterDirection.Input);
                parameters.Add("@PageNumber", pageNumber, DbType.Int32, direction: ParameterDirection.Input);
                parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                try
                {


                    var results = connection.QueryAsync<Place>("SearchPlaces", parameters, commandType: CommandType.StoredProcedure);
                    var totalCount = parameters.Get<int>("TotalCount");

                    return new PlaceSearchResult
                    {
                        TotalCount = totalCount,
                        Places = await results
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return await Task.FromResult<PlaceSearchResult>(null);

                }


            }
        }
    }
}
        
        








        


 
    


        

    



