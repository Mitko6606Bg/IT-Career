using System.ComponentModel.DataAnnotations;

namespace TestMVC.Data.Entities
{
    public class Car
    {
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [Required]
        public string Model { get; set; }
        [Required]
        public string Color { get; set; }
    }
}
