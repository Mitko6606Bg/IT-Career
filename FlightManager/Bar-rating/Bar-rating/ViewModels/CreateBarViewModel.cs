using System.ComponentModel.DataAnnotations;

namespace Bar_rating.ViewModels
{
    public class CreateBarViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

    }
}
