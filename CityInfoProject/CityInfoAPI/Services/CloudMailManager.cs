namespace CityInfoAPI.Services
{
    public class CloudMailManager : IMailService
    {
        public void SendMail(string sender, string receiver, string message)
        {
            Console.WriteLine("..............Cloud Send Mail.................");
            Console.WriteLine($"Sender: {sender} via Cloud");
            Console.WriteLine($"Receiver: {receiver} via Cloud");
            Console.WriteLine(message);
        }
    }
}
