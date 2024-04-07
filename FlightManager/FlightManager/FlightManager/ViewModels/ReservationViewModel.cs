using System.ComponentModel.DataAnnotations;

namespace FlightManager.ViewModels
{
    public class ReservationViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string EGN { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public string TypeOfReservation { get; set; }
    }
}
