namespace FlightManager.Models
{
    public class UserDetails
    {
        public IEnumerable<UserDetails> Users { get; set; }  // Property to hold the collection of users

        public string Id { get; set; }
        public string Name { get; set; }
        public string Last_Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string EGN { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
