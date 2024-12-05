using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Bar_rating.Data.Entities
{
    public class Review
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int Rating { get; set; }
        public string RatingText { get; set; }


        public int BarId { get; set; }
        public Bar Bar { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
