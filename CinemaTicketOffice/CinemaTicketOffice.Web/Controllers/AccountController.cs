using CinemaTicketOffice.Domain.DTO.Identity;
using CinemaTicketOffice.Domain.Models.Domain;
using CinemaTicketOffice.Domain.Models.Identity;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
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
                    EmailConfirmed = true,
                    ShoppingCart = new Domain.Models.Domain.ShoppingCart()
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

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> ImportUsers(IFormFile file)
        {
            if (file == null)
                return RedirectToAction("Index");

            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\Files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<UserImportDTO> items = GetUsersFromExcelFile(file.FileName);
            foreach (UserImportDTO item in items)
            {
                TicketOfficeUser user = await _userManager.FindByEmailAsync(item.Email);
                if (user != null)
                    continue;

                user = new TicketOfficeUser()
                {
                    UserName = item.Email,
                    NormalizedUserName = item.Email,
                    Email = item.Email,
                    NormalizedEmail = item.Email,
                    EmailConfirmed = true,
                    ShoppingCart = new ShoppingCart()
                };

                IdentityResult result = await _userManager.CreateAsync(user, item.Password);
                if (!result.Succeeded)
                    continue;

                result = await _userManager.AddToRoleAsync(user, item.Role);
                if (!result.Succeeded)
                    continue;
            }

            return RedirectToAction("Index");
        }


        private List<UserImportDTO> GetUsersFromExcelFile(string fileName)
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\Files\\{fileName}";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            List<UserImportDTO> userList = new List<UserImportDTO>();
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        userList.Add(new UserImportDTO
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            Role = reader.GetValue(2).ToString()
                        });
                    }
                }
            }

            return userList;
        }
    }
}
