using CityInfoAPI.Models.Entities;

namespace CityInfoAPI.Services
{
    public interface IAuthenticationService
    {
       User ValidateCredentials(string username, string password);
    }
}
