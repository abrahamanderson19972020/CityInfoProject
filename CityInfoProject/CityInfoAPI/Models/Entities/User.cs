namespace CityInfoAPI.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }

        public User(int userId, string username,string firstname,string lastname, string address)
        {
            UserId = userId;
            Username = username;
            Firstname = firstname;
            Lastname = lastname;
            Address = address;
        }
    }
}
