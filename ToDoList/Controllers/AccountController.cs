using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userMenager;
        private readonly SignInManager<AppUser> _signInMenager;


        public AccountController(UserManager<AppUser> userMenager, SignInManager<AppUser> signInMenager)
        {
            _userMenager = userMenager;
            _signInMenager = signInMenager;
        }
        
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = email, Email = email };
                var result = await _userMenager.CreateAsync(user,password);

                if (result.Succeeded) { 
                    await _signInMenager.SignInAsync(user, isPersistent:false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View("Register");
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInMenager.PasswordSignInAsync(email, password, rememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Twoje konto jest zablokowane.");
                    return View();
                }
                ModelState.AddModelError("", "Nieprawidłowy login lub hasło.");
            }
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInMenager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
