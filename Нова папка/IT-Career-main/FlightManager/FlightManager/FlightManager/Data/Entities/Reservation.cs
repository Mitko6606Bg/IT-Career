using System.ComponentModel.DataAnnotations;

namespace FlightManager.Data.Entities
{
    public class Reservation
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FlightNumber { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }
}
