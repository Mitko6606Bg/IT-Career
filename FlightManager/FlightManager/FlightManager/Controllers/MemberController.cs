using FlightManager.Data;
using FlightManager.Models;
using FlightManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightManager.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public MemberController(UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _context = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            // Find reservations and tickets associated with the user's email
            var reservations = await _context.Reservations.Where(r => r.Email == user.Email).ToListAsync();

            // Create a list to hold ReservationAndTicketViewModel instances
            var reservationAndTicketViewModels = new List<ReservationAndTicketViewModel>();

            // Populate the list with ReservationAndTicketViewModel instances for each reservation
            foreach (var reservation in reservations)
            {
                // Find the tickets associated with the reservation
                var tickets = await _context.Tickets.Where(t => t.ReservationId == reservation.Id).ToListAsync();

                // Create a list to hold TicketViewModel instances
                var ticketViewModels = new List<TicketViewModel>();

                // Populate the list with TicketViewModel instances for each ticket
                foreach (var ticket in tickets)
                {
                    var ticketViewModel = new TicketViewModel
                    {
                        FirstName = ticket.FirstName,
                        LastName = ticket.LastName,
                        EGN = ticket.EGN,
                        PhoneNumber = ticket.PhoneNumber,
                        Nationality = ticket.Nationality,
                        TypeOfReservation = ticket.TypeOfReservation
                    };

                    ticketViewModels.Add(ticketViewModel);
                }

                // Create a ReservationAndTicketViewModel and populate it with reservation email and its tickets
                var reservationAndTicketViewModel = new ReservationAndTicketViewModel
                {
                    Reservation = new ReservationViewModel { Email = reservation.Email }, // Only assign the email
                    Tickets = ticketViewModels
                };

                reservationAndTicketViewModels.Add(reservationAndTicketViewModel);
            }

            // Create a UserReservationTicketViewModel and populate it with user information and reservation/ticket details
            var viewModel = new UserReservationTicketViewModel
            {
                User = new ManageViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Last_Name = user.Last_Name,
                    Username = user.UserName,
                    EGN = user.EGN,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber
                    // Add other properties you need to manage
                },
                ReservationAndTicket = reservationAndTicketViewModels
            };

            return View(viewModel);
        }

    }
}
