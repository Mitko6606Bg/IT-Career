using System.ComponentModel.DataAnnotations;

namespace FlightManager.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password don't match.")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Last_Name { get; set; }
        public string? EGN { get; set; }
        [DataType(DataType.MultilineText)]
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
