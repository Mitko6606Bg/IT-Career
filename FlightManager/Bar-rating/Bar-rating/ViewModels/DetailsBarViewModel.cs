using System.ComponentModel.DataAnnotations;

namespace Bar_rating.ViewModels
{
    public class DetailsBarViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public byte[] Image { get; set; }
    }
}
