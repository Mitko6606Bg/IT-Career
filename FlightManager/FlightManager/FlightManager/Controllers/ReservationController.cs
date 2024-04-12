using System.Linq;
using System.Threading.Tasks;
using FlightManager.Data;
using FlightManager.Data.Entities;
using FlightManager.Models;
using FlightManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightManager.Controllers
{
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ReservationController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            string userEmail = TempData["UserEmail"] as string;

            // Pass the email address to the view
            ViewData["UserEmail"] = userEmail;

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ReservationsIndex(int pageSize = 10, int pageNumber = 1)
        {
            var reservations = await _context.Reservations
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(reservations);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new ReservationAndTicketViewModel();

            if (User.Identity.IsAuthenticated)
            {
                // Get the email of the logged-in user
                var user = await _userManager.GetUserAsync(User);
                var userEmail = user?.Email;

                // Populate the email field in the view model
                model.Reservation.Email = userEmail;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationAndTicketViewModel model , int id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (User.Identity.IsAuthenticated)
            {
                // Get the email of the logged-in user
                var user = await _userManager.GetUserAsync(User);
                var userEmail = user?.Email;

                // Assign the logged-in user's email to the reservation
                model.Reservation.Email = userEmail;
            }

            // Get the flight associated with the reservation
            var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == id);

            if (flight == null)
            {
                return NotFound("Flight not found");
            }


            var passengerTicketsCount = await _context.Tickets
                .CountAsync(t => t.Reservation.FlightNumber == id.ToString() && t.TypeOfReservation == "Normal");

            var businessTicketsCount = await _context.Tickets
                .CountAsync(t => t.Reservation.FlightNumber == id.ToString() && t.TypeOfReservation == "Business");

            var availablePassengerSeats = flight.PassengerCapacity;
            var availableBusinessSeats = flight.BusinessClassCapacity;
            int TicketsCount = model.Tickets.Count;
            int BusinessTicketsCount = model.Tickets.Count(t => t.TypeOfReservation == "Business");

            if (availablePassengerSeats < TicketsCount || availableBusinessSeats < BusinessTicketsCount)
            {
                // Not enough free seats
                TempData["SuccessMessageReservationCreate"] = $"There are not enough free seats available on this flight. Only {flight.PassengerCapacity} left from which {flight.BusinessClassCapacity} Business";
                return View(model);
            }
            var newReservation = new Reservation
            {
                Email = model.Reservation.Email,
                FlightNumber = id.ToString(),
            };

            // Add the new reservation to the database
            _context.Reservations.Add(newReservation);
            await _context.SaveChangesAsync();

            // Iterate over the list of TicketViewModel and create Ticket entities
            foreach (var ticketViewModel in model.Tickets)
            {
                var newTicket = new Ticket
                {
                    FirstName = ticketViewModel.FirstName,
                    LastName = ticketViewModel.LastName,
                    EGN = ticketViewModel.EGN,
                    PhoneNumber = ticketViewModel.PhoneNumber,
                    Nationality = ticketViewModel.Nationality,
                    TypeOfReservation = ticketViewModel.TypeOfReservation,
                    ReservationId = newReservation.Id // Assign the reservation ID to the ticket
                };

                // Add the ticket to the reservation
                newReservation.Tickets.Add(newTicket);
            }

            // Update the database to save the changes to tickets
            

            flight.PassengerCapacity -= TicketsCount;
            flight.BusinessClassCapacity -= BusinessTicketsCount;
            await _context.SaveChangesAsync();

            var emailService = new EmailService();
            var subject = "Flight Reservation Confirmation"; // Email subject
            var body = @"
                <html>
                <head>
                    <style>
                        /* CSS styles for the email body */
                        /* You can add your custom styles here */
                    </style>
                </head>
                <body>
                    <h1>Thank you for your reservation!</h1>
                    <p>Here are the details of your reservation:</p>
                    <ul>
                        <li><strong>Email:</strong> " + model.Reservation.Email + @"</li>
                        <li><strong>Flight Number:</strong> " + id + @"</li>
                        <li><strong>Reservation ID:</strong> " + newReservation.Id + @"</li>";


            int countTickets = 0;
            // Iterate over the list of tickets and add details for each ticket
            foreach (var ticket in model.Tickets)
            {
                countTickets++;
                body += @"
                        <li>
                            <strong>Ticket Details (" + countTickets + @"):</strong>
                            <ul>
                                <li><strong>First Name:</strong> " + ticket.FirstName + @"</li>
                                <li><strong>Last Name:</strong> " + ticket.LastName + @"</li>
                                <li><strong>EGN:</strong> " + ticket.EGN + @"</li>
                                <li><strong>Phone Number:</strong> " + ticket.PhoneNumber + @"</li>
                                <li><strong>Nationality:</strong> " + ticket.Nationality + @"</li>
                                <li><strong>Type of Reservation:</strong> " + ticket.TypeOfReservation + @"</li>
                            </ul>
                        </li>";
                        }
                
                        body += @"
                    </ul>
                </body>
                </html>";

            // Send reservation confirmation email
            await emailService.SendReservationEmailAsync(model.Reservation.Email, subject, body);



            TempData["SuccessMessageReservationCreate"] = "Reservation created successfully.";
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Manage(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            // Find reservations associated with the user's email
            var reservations = await _context.Reservations.Where(r => r.Email == user.Email).ToListAsync();
            var reservationViewModels = new List<ReservationViewModel>();

            foreach (var reservation in reservations)
            {
                // Find tickets associated with the reservation
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

                // Create a ReservationViewModel and populate it with reservation email and its tickets
                var reservationViewModel = new ReservationViewModel
                {
                    Email = reservation.Email,
                };

                reservationViewModels.Add(reservationViewModel);
            }

            var viewModel = new ManageViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Last_Name = user.Last_Name,
                Username = user.UserName,
                EGN = user.EGN,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Reservation deleted successfully.";
            return RedirectToAction(nameof(ReservationsIndex));
        }
    }
}
