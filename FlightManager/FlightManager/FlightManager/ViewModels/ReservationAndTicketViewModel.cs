namespace FlightManager.ViewModels
{
    public class ReservationAndTicketViewModel
    {
        public ReservationViewModel Reservation { get; set; }
        public List<TicketViewModel> Tickets { get; set; }
        public ReservationAndTicketViewModel()
        {
            Reservation = new ReservationViewModel();
            Tickets = new List<TicketViewModel>();
        }

    }
}
