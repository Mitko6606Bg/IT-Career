using System.ComponentModel.DataAnnotations;

namespace FlightManager.Data.Entities
{
    public class User
    {
        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? EGN { get; set; }

        [Required]
        public string? Address { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
