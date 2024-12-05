using System.ComponentModel.DataAnnotations;

namespace Bar_rating.Data.Entities
{
    public class Bar
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public ICollection<Review> Reviews { get; set; }

    }
}
