using FlightManager.Models;
using FlightManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlightManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> UsersIndex(int pageSize = 10, int pageNumber = 1)
        {
            var users = _userManager.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var userDetails = users.Select(u => new AppUser
            {
                Id = u.Id,
                Name = u.Name,
                UserName = u.UserName,
                Email = u.Email,
                EGN = u.EGN,
                PhoneNumber = u.PhoneNumber,
            });

            return View(userDetails);
        }


        public async Task<IActionResult> Manage(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ManageViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Last_Name = user.Last_Name,
                Username = user.UserName,
                Email = user.Email,
                EGN = user.EGN,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
                // Add other properties you need to manage
            };

            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> Manage(ManageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = model.Name;
            user.Last_Name = model.Last_Name;
            user.UserName = model.Username;
            user.Email = model.Email;
            user.EGN = model.EGN;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Changes saved successfully.";
                return RedirectToAction("Manage", "Admin"); // Redirect to some action after successful update
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }


        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return RedirectToAction(nameof(UsersIndex));
        }
    }


}
