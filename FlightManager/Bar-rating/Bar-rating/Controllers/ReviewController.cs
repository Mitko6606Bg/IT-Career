using Bar_rating.Data;
using Bar_rating.Data.Entities;
using Bar_rating.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bar_rating.Controllers
{
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context; 

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> EditReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            var editViewModel = new EditReviewViewModel
            {
                Id = review.Id,
                Rating = review.Rating.ToString(),
                RatingText = review.RatingText
            };

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview(int id, EditReviewViewModel editViewModel)
        {
            if (id != editViewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var review = await _context.Reviews.FindAsync(id);
                    if (review == null)
                    {
                        return NotFound();
                    }

                    review.Rating = int.Parse(editViewModel.Rating);
                    review.RatingText = editViewModel.RatingText;

                    _context.Update(review);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("MyReviews", "Account");
                }
                catch (DbUpdateException)
                {
                    if (!ReviewExists(editViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReview(int id)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            _context.SaveChanges();

            return RedirectToAction("MyReviews","Account");
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
