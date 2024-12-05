using Bar_rating.Data.Entities;
using Bar_rating.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bar_rating.ViewModels;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Identity;

namespace Bar_rating.Controllers
{
    public class BarController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BarController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_context.Bars.ToList());

        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bar = _context.Bars.FirstOrDefault(m => m.Id == id);
            if (bar == null)
            {
                return NotFound();
            }

            // Create a new instance of the view model
            var viewModel = new BarDetailsWithReviewViewModel
            {
                Id = bar.Id,
                Name = bar.Name,
                Description = bar.Description,
              
            };

            // Check if there are reviews available for this bar
            var reviews = _context.Reviews.Where(r => r.BarId == id).ToList();
            if (reviews.Any())
            {
                // Calculate the average rating from reviews
                var averageRating = (int)Math.Round(reviews.Average(r => r.Rating));

                // Set the average rating in the view model
                viewModel.Rating = averageRating;
            }
            else
            {
                // If no reviews available, set default values or leave null
                viewModel.Rating = 0; // Default value or null
                viewModel.RatingText = null; // Default value or null
            }

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReview(int id, string ratingText, int rating)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(User);

                var review = new Review
                {
                    Rating = rating,
                    RatingText = ratingText,
                    BarId = id, 
                    UserId = user.Id

                              
                };

                // Add the review to the database context
                _context.Reviews.Add(review);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));



            }

            // If model state is not valid, return to the details view with the same id
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBarViewModel model)
        {
            if (ModelState.IsValid)
            {
                

                var bar = new Bar
                {
                    Name = model.Name,
                    Description = model.Description,
                };

                _context.Add(bar);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bar = _context.Bars.FirstOrDefault(m => m.Id == id);
            if (bar == null)
            {
                return NotFound();
            }
            var editViewModel = new EditBarViewModel
            {
                Id = bar.Id,
                Name = bar.Name,
                Description = bar.Description,
                //Image = bar.Image
            };

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditBarViewModel editViewModel)
        {
            if (id != editViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing bar entity from the database
                    var bar = _context.Bars.FirstOrDefault(b => b.Id == id);
                    if (bar == null)
                    {
                        return NotFound();
                    }

                    // Update the properties that can be changed without considering the image
                    bar.Name = editViewModel.Name;
                    bar.Description = editViewModel.Description;

 
                    _context.Update(bar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BarExists(editViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(editViewModel);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bar = _context.Bars.FirstOrDefault(m => m.Id == id);
            if (bar == null)
            {
                return NotFound();
            }

            return View(bar);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var bar = _context.Bars.Find(id);
            _context.Bars.Remove(bar);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool BarExists(int id)
        {
            return _context.Bars.Any(e => e.Id == id);
        }
    }
}
