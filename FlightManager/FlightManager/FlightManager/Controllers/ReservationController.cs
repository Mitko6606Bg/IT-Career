using System.Linq;
using System.Threading.Tasks;
using FlightManager.Data;
using FlightManager.Data.Entities;
using FlightManager.Models;
using FlightManager.ViewModels;
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
        public async Task<IActionResult> Create(ReservationViewModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData["UserEmail"] = model.Email;

            var newReservation = new Reservation
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                EGN = model.EGN,
                PhoneNumber = model.PhoneNumber,
                Nationality = model.Nationality,
                TypeOfReservation = model.TypeOfReservation,
                FlightNumber = id.ToString()
            };

            _context.Reservations.Add(newReservation);
            await _context.SaveChangesAsync();

            var emailService = new EmailService();
            var subject = "Flight Reservation Confirmation"; // Email subject
            var body = $@"
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
                        <li><strong>First Name:</strong> {model.FirstName}</li>
                        <li><strong>Last Name:</strong> {model.LastName}</li>
                        <li><strong>Email:</strong> {model.Email}</li>
                        <li><strong>EGN:</strong> {model.EGN}</li>
                        <li><strong>Phone Number:</strong> {model.PhoneNumber}</li>
                        <li><strong>Nationality:</strong> {model.Nationality}</li>
                        <li><strong>Type of Reservation:</strong> {model.TypeOfReservation}</li>
                    </ul>
                </body>
                </html>";

            // Send reservation confirmation email
            await emailService.SendReservationEmailAsync(model.Email, subject, body);



            TempData["SuccessMessage"] = "Reservation created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Manage(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(Reservation model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var reservation = await _context.Reservations.FindAsync(model.Id);

            if (reservation == null)
            {
                return NotFound();
            }

            reservation.FirstName = model.FirstName;
            reservation.LastName = model.LastName;
            reservation.EGN = model.EGN;
            reservation.PhoneNumber = model.PhoneNumber;
            reservation.Nationality = model.Nationality;
            reservation.TypeOfReservation = model.TypeOfReservation;
            reservation.FlightNumber = model.FlightNumber;

            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Changes saved successfully.";

            return RedirectToAction(nameof(Manage));
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
