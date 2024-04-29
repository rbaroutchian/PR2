namespace PlaceReserv.Services
{
    public interface ILoginService
    {
        bool Authenticate(string username, string password);
        string GenerateToken(string username);
        

    }
}