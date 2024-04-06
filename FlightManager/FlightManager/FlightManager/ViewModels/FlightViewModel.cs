using System.ComponentModel.DataAnnotations;

namespace FlightManager.ViewModels
{
    public class FlightViewModel
    {
        public int Id { get; set; }
        public string FromLocation { get; set; }

        public string ToLocation { get; set; }

        public DateTime DepartureDateTime { get; set; }

        public DateTime LandingDateTime { get; set; }

        public string AircraftType { get; set; }

        public string AircraftNumber { get; set; }

        public string PilotName { get; set; }

        public int PassengerCapacity { get; set; }

        public int BusinessClassCapacity { get; set; }
    }
}
