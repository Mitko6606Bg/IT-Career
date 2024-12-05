using Bar_rating.Data;
using Bar_rating.Data.Entities;
using Bar_rating.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bar_rating.Controllers
{
    public class AccountController : Controller
    {

        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = appDbContext;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (User.IsInRole("Member"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Name = model.Name,
                    LastName = model.LastName,

                };

                var result = await _userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddToRoleAsync(user, "Member");
                    Logout();
                    return RedirectToAction("Login", "Account");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            Logout();
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> MyReviews()
        {

            var user = await _userManager.GetUserAsync(User);
            var myReviews = _context.Reviews.Where(r => r.UserId == user.Id).ToList();

            return View(myReviews);
        }


    }
}
