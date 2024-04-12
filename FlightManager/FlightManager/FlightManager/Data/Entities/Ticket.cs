using System.ComponentModel.DataAnnotations;

namespace FlightManager.Data.Entities
{
    public class Ticket
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ReservationId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EGN { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]
        public string TypeOfReservation { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();

        

        public Reservation Reservation { get; set; } // Navigation property
    }
}
