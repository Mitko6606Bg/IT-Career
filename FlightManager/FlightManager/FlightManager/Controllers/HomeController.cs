using FlightManager.Data;
using FlightManager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FlightManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context; 


        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var flights = _context.Flights.ToList();

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

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Flights(int pageSize = 10, int pageNumber = 1)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
