using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bar_rating.Data.Entities
{
    public class AppUser : IdentityUser
    {
        [StringLength(100)]
        [MaxLength(100)]
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? LastName { get; set; }
    }
}
