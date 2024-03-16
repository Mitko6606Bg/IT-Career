using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FlightManager.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(100)]
        [MaxLength(100)]
        [Required]
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Last_Name { get; set; }
        public string? EGN { get; set; }
    }
}
