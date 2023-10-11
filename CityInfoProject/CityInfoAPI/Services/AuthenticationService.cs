using CityInfoAPI.Models.Entities;

namespace CityInfoAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public User ValidateCredentials(string username, string password)
        {
            return new User(1, "sd", "sds","sds","sds");
        }
    }
}
