using System.ComponentModel.DataAnnotations;

namespace Bar_rating.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [StringLength(100)]
        [MaxLength(100)]
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The LastName field is required.")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "The Email field is required.")]
        public string Email { get; set; }
    }
}
