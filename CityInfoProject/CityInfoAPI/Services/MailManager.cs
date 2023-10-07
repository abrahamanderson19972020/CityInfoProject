namespace CityInfoAPI.Services
{
    public class MailManager : IMailService
    {
        public string EmailSender { get; set; }
        public string EmailReceiver { get; set; } 
        public void SendMail(string sender, string receiver,string message)
        {
            Console.WriteLine("..............Local Send Mail.................");
            Console.WriteLine($"Sender: {sender}");
            Console.WriteLine($"Receiver: {receiver}");
            Console.WriteLine(message);
        }
    }
}
