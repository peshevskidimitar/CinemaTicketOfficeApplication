using CinemaTicketOffice.Domain.DTO.Identity;
using CinemaTicketOffice.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CinemaTicketOffice.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<TicketOfficeUser> _userManager;
        private readonly SignInManager<TicketOfficeUser> _signInManager;

        public AccountController(UserManager<TicketOfficeUser> userManager, SignInManager<TicketOfficeUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Register()
        {
            return View(new UserRegistrationDTO());
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationDTO request)
        {
            if (ModelState.IsValid)
            {
                TicketOfficeUser user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    ModelState.AddModelError("message", "User with such email already exists.");
                    return View(request);
                }

                user = new TicketOfficeUser()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.Email,
                    NormalizedUserName = request.Email,
                    Email = request.Email,
                    NormalizedEmail = request.Email,
                    EmailConfirmed = true
                };

                IdentityResult result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(user, "Regular");
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("message", "Invalid login attempt.");
                        return View(request);
                    }

                    return RedirectToAction("Login");
                }
                    
                if (result.Errors.Count() > 0)
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("message", error.Description);
            }

            return View(request);
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            return View(new UserLoginDTO());
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDTO request)
        {
            if (ModelState.IsValid)
            {
                TicketOfficeUser user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    ModelState.AddModelError("message", "A user with such email does not exist.");
                    return View(request);
                }

                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "The email is not confirmed.");
                    return View(request);
                }

                if (await _userManager.CheckPasswordAsync(user, request.Password) == false)
                {
                    ModelState.AddModelError("message", "The password is incorrect.");
                    return View(request);
                }

                Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, true);
                if (!signInResult.Succeeded)
                {
                    ModelState.AddModelError("message", "Invalid login attempt.");
                    return View(request);
                }

                return RedirectToAction("Index", "Home");
            }

            return View(request);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
