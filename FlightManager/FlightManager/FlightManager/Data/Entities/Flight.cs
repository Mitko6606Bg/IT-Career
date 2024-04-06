using System.ComponentModel.DataAnnotations;

namespace FlightManager.Data.Entities
{
    public class Flight
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string FromLocation { get; set; }

        [Required]
        public string ToLocation { get; set; }

        [Required]
        public DateTime DepartureDateTime { get; set; }

        [Required]
        public DateTime LandingDateTime { get; set; }

        [Required]
        public string AircraftType { get; set; }

        [Required]
        public string AircraftNumber { get; set; }

        [Required]
        public string PilotName { get; set; }

        [Required]
        public int PassengerCapacity { get; set; }

        [Required]
        public int BusinessClassCapacity { get; set; }
    }
}
