namespace CityInfoAPI.Services
{
    public interface IMailService
    {
        void SendMail(string sender,string receiver, string message);
    }
}
