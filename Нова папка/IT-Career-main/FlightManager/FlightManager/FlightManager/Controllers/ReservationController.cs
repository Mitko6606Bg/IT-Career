using System.Linq;
using System.Threading.Tasks;
using FlightManager.Data;
using FlightManager.Data.Entities;
using FlightManager.Models;
using FlightManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightManager.Controllers
{
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;

        public ReservationController(AppDbContext context)
        {
            _context = context;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationAndTicketViewModel model , int id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Create a new Reservation entity
            var newReservation = new Reservation
            {
                Email = model.Reservation.Email,
                // Set other reservation properties as needed
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

        //public async Task<IActionResult> Manage(int id)
        //{
        //    var reservation = await _context.Reservations.FindAsync(id);

        //    if (reservation == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(reservation);
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public async Task<IActionResult> Manage(ReservationAndTicketViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // Find the reservation by its ID
        //    var reservation = await _context.Reservations.FindAsync(model.Reservation.Id);

        //    if (reservation == null)
        //    {
        //        return NotFound();
        //    }

        //    // Update the reservation details
        //    reservation.Email = model.Reservation.Email;
        //    // Update other reservation properties as needed

        //    // Clear existing tickets associated with the reservation
        //    reservation.Tickets.Clear();

        //    // Iterate over the list of tickets in the view model and add them to the reservation
        //    foreach (var ticketViewModel in model.Tickets)
        //    {
        //        var newTicket = new Ticket
        //        {
        //            FirstName = ticketViewModel.FirstName,
        //            LastName = ticketViewModel.LastName,
        //            EGN = ticketViewModel.EGN,
        //            PhoneNumber = ticketViewModel.PhoneNumber,
        //            Nationality = ticketViewModel.Nationality,
        //            TypeOfReservation = ticketViewModel.TypeOfReservation,
        //            ReservationId = reservation.Id // Assign the reservation ID to the ticket
        //        };

        //        reservation.Tickets.Add(newTicket);
        //    }

        //    // Save the changes to the database
        //    await _context.SaveChangesAsync();

        //    TempData["SuccessMessage"] = "Changes saved successfully.";

        //    return RedirectToAction(nameof(Manage));
        //}


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
