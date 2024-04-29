using PlaceReserv.Controllers;
using PlaceReserv.Models;

namespace PlaceReserv.Services
{
    public interface IRegistrationService
    {
        bool Register(User user);

    }
}
