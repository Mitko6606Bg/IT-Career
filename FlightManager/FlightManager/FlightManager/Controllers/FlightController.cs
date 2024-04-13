using FlightManager.Data;
using FlightManager.Data.Entities;
using FlightManager.Models;
using FlightManager.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightManager.Controllers
{
    public class FlightController : Controller
    {
        private readonly AppDbContext _context; // Replace YourDbContext with your actual DbContext type

        public FlightController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FlightsIndex(int pageSize = 10, int pageNumber = 1)
        {
            var flights = _context.Flights
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var flightDetails = flights.Select(u => new FlightViewModel
            {
                Id = u.Id,
                FromLocation = u.FromLocation,
                ToLocation = u.ToLocation,
                DepartureDateTime = u.DepartureDateTime,
                LandingDateTime = u.LandingDateTime,
                AircraftType = u.AircraftType,
                AircraftNumber = u.AircraftNumber,
                PilotName = u.PilotName,
                PassengerCapacity = u.PassengerCapacity,
                BusinessClassCapacity = u.BusinessClassCapacity
                ,
            });

            return View(flightDetails);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FlightViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (ModelState.IsValid)
            {
                // Compare departure and landing date-times
                if (model.LandingDateTime <= model.DepartureDateTime)
                {
                    // Landing date-time must be after departure date-time
                    TempData["FailMessage"] = "Landing date and time must be after departure date and time.";
                    return RedirectToAction(nameof(Create)); // Redirect to the create page
                }
            }


            TimeSpan duration = model.LandingDateTime - model.DepartureDateTime;
            

            // Your code to create a new flight goes here
            var newFlight = new Flight
            {
                FromLocation = model.FromLocation,
                ToLocation = model.ToLocation,
                DepartureDateTime = model.DepartureDateTime,
                LandingDateTime = model.LandingDateTime,
                AircraftType = model.AircraftType,
                AircraftNumber = model.AircraftNumber,
                PilotName = model.PilotName,
                PassengerCapacity = model.PassengerCapacity,
                BusinessClassCapacity = model.BusinessClassCapacity,
                Duration = duration,
            };

            // Example: Save the new flight to the database using Entity Framework Core
            _context.Flights.Add(newFlight);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Flight created successfully.";
            return RedirectToAction(nameof(FlightsIndex));
        }


        public async Task<IActionResult> Manage(int id, FlightReservationTicketViewModel viewModelUpdate)
        {
            // Find the reservations with the given flight number
            var reservations = await _context.Reservations
                .Where(r => r.FlightNumber == id.ToString())
                .ToListAsync();

            // For simplicity, I'll assume there's only one reservation per flight
            var reservation = reservations.FirstOrDefault();

            // Find the associated flight for the reservation
            var flight = await _context.Flights.FindAsync(id);

            FlightReservationTicketViewModel viewModel;

            if (reservation == null)
            {
                // Render the view without the reservation and tickets part
                viewModel = new FlightReservationTicketViewModel
                {
                    Flight = new FlightViewModel
                    {
                        // Populate flight properties as needed
                        // For example:
                        Id = flight.Id,
                        FromLocation = flight.FromLocation,
                        ToLocation = flight.ToLocation,
                        DepartureDateTime = flight.DepartureDateTime,
                        LandingDateTime = flight.LandingDateTime,
                        AircraftType = flight.AircraftType,
                        PilotName = flight.PilotName,
                        PassengerCapacity = flight.PassengerCapacity,
                        BusinessClassCapacity = flight.BusinessClassCapacity,
                        AircraftNumber = flight.AircraftNumber
                    },
                    ReservationAndTicket = null // Set ReservationAndTicket to null when there are no reservations
                };

                if (!ModelState.IsValid)
                {
                    // Handle invalid ModelState for AircraftNumber
                    // For example:
                    ModelState.Remove("ReservationAndTicket"); // Remove AircraftNumber from ModelState
                }
                if (ModelState.IsValid)
                {
                    flight.FromLocation = viewModelUpdate.Flight.FromLocation;
                    flight.ToLocation = viewModelUpdate.Flight.ToLocation;
                    flight.DepartureDateTime = viewModelUpdate.Flight.DepartureDateTime;
                    flight.LandingDateTime = viewModelUpdate.Flight.LandingDateTime;
                    flight.AircraftType = viewModelUpdate.Flight.AircraftType;
                    flight.PilotName = viewModelUpdate.Flight.PilotName;
                    flight.PassengerCapacity = viewModelUpdate.Flight.PassengerCapacity;
                    flight.BusinessClassCapacity = viewModelUpdate.Flight.BusinessClassCapacity;
                    flight.AircraftNumber = viewModelUpdate.Flight.AircraftNumber;

                    // Save changes to the flight
                    _context.Flights.Update(flight);
                    await _context.SaveChangesAsync();
                }

                return View(viewModel);
            }

            FlightViewModel flightView = new FlightViewModel
            {
                // Populate flight properties as needed
                // For example:
                Id = flight.Id,
                FromLocation = flight.FromLocation,
                ToLocation = flight.ToLocation,
                DepartureDateTime = flight.DepartureDateTime,
                LandingDateTime = flight.LandingDateTime,
                AircraftType = flight.AircraftType,
                PilotName = flight.PilotName,
                PassengerCapacity = flight.PassengerCapacity,
                BusinessClassCapacity = flight.BusinessClassCapacity,
                AircraftNumber = flight.AircraftNumber
            };

            if (!ModelState.IsValid)
            {
                // Handle invalid ModelState for AircraftNumber
                // For example:
                ModelState.Remove("ReservationAndTicket"); // Remove AircraftNumber from ModelState
            }
            if (ModelState.IsValid)
            {
                flight.FromLocation = viewModelUpdate.Flight.FromLocation;
                flight.ToLocation = viewModelUpdate.Flight.ToLocation;
                flight.DepartureDateTime = viewModelUpdate.Flight.DepartureDateTime;
                flight.LandingDateTime = viewModelUpdate.Flight.LandingDateTime;
                flight.AircraftType = viewModelUpdate.Flight.AircraftType;
                flight.PilotName = viewModelUpdate.Flight.PilotName;
                flight.PassengerCapacity = viewModelUpdate.Flight.PassengerCapacity;
                flight.BusinessClassCapacity = viewModelUpdate.Flight.BusinessClassCapacity;
                flight.AircraftNumber = viewModelUpdate.Flight.AircraftNumber;

                // Save changes to the flight
                _context.Flights.Update(flight);
                await _context.SaveChangesAsync();
            }

            var tickets = await _context.Tickets.Where(t => t.ReservationId == reservation.Id).ToListAsync();

            // Create a list to hold ReservationAndTicketViewModel instances
            var reservationAndTicketViewModels = new List<ReservationAndTicketViewModel>();

            // Populate the list with ReservationAndTicketViewModel instances for each reservation
            foreach (var res in reservations)
            {
                var reservationViewModel = new ReservationViewModel
                {
                    Email = res.Email
                };

                var ticketViewModels = new List<TicketViewModel>();
                var resTickets = await _context.Tickets.Where(t => t.ReservationId == res.Id).ToListAsync();

                // Populate the list with TicketViewModel instances for each ticket
                foreach (var ticket in resTickets)
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

                var reservationAndTicketViewModel = new ReservationAndTicketViewModel
                {
                    Reservation = reservationViewModel,
                    Tickets = ticketViewModels
                };

                reservationAndTicketViewModels.Add(reservationAndTicketViewModel);
            }

            // Populate view model with flight and reservationAndTicketViewModels

            viewModel = new FlightReservationTicketViewModel
            {
                Flight = flightView,
                ReservationAndTicket = reservationAndTicketViewModels
            };


            // Redirect to a success action or return a success message
            TempData["SuccessMessage"] = "Flight details updated successfully!";
            return View(viewModel);
        }



        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            // Remove the flight from the database
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Flight deleted successfully.";
            return RedirectToAction(nameof(FlightsIndex));
        }
    }
}
