using System.ComponentModel.DataAnnotations;

namespace FlightManager.ViewModels
{
    public class TicketViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "EGN must be exactly 10 digits.")]
        public string EGN { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public string TypeOfReservation { get; set; }
    }
}

