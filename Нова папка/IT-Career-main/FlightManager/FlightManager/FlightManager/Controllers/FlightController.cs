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
                BusinessClassCapacity = model.BusinessClassCapacity
            };

            // Example: Save the new flight to the database using Entity Framework Core
            _context.Flights.Add(newFlight);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Flight created successfully.";
            return RedirectToAction(nameof(FlightsIndex));
        }

        public async Task<IActionResult> Manage(int id)
        {
            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            // Map flight details to view model
            var viewModel = new FlightViewModel
            {
                Id = flight.Id,
                FromLocation = flight.FromLocation,
                ToLocation = flight.ToLocation,
                DepartureDateTime = flight.DepartureDateTime,
                LandingDateTime = flight.LandingDateTime,
                AircraftType = flight.AircraftType,
                AircraftNumber = flight.AircraftNumber,
                PilotName = flight.PilotName,
                PassengerCapacity = flight.PassengerCapacity,
                BusinessClassCapacity = flight.BusinessClassCapacity
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(FlightViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Retrieve flight from the database by its ID
            var flight = await _context.Flights.FindAsync(model.Id);

            if (flight == null)
            {
                return NotFound();
            }

            // Update flight details with values from the view model
            flight.FromLocation = model.FromLocation;
            flight.ToLocation = model.ToLocation;
            flight.DepartureDateTime = model.DepartureDateTime;
            flight.LandingDateTime = model.LandingDateTime;
            flight.AircraftType = model.AircraftType;
            flight.AircraftNumber = model.AircraftNumber;
            flight.PilotName = model.PilotName;
            flight.PassengerCapacity = model.PassengerCapacity;
            flight.BusinessClassCapacity = model.BusinessClassCapacity;

            // Save changes to the database
            _context.Entry(flight).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Changes saved successfully.";

            return RedirectToAction(nameof(Manage));
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
