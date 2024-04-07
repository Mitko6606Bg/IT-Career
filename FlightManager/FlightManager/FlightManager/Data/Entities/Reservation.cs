using System.ComponentModel.DataAnnotations;

namespace FlightManager.Data.Entities
{
    public class Reservation
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string EGN { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]
        public string TypeOfReservation { get; set; }
        [Required]
        public string FlightNumber { get; set; }

    }
}
