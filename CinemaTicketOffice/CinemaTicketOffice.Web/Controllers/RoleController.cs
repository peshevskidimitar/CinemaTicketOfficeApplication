using CinemaTicketOffice.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaTicketOffice.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<TicketOfficeUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<TicketOfficeUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            List<TicketOfficeUser> members = new List<TicketOfficeUser>();
            List<TicketOfficeUser> nonMembers = new List<TicketOfficeUser>();
            foreach (TicketOfficeUser user in _userManager.Users)
                if (await _userManager.IsInRoleAsync(user, role.Name))
                    members.Add(user);
                else
                    nonMembers.Add(user);

            return View(new RoleEdit()
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoleModification request)
        {
            if (ModelState.IsValid)
            {
                foreach (string userId in request.AddIds ?? new string[] {})
                {
                    TicketOfficeUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        IdentityResult result = await _userManager.AddToRoleAsync(user, request.RoleName);
                        if (!result.Succeeded)
                            ModelState.AddModelError("message", "Cannot add user to role.");
                    }
                }

                foreach (string userId in request.DeleteIds ?? new string[] {})
                {
                    TicketOfficeUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        IdentityResult result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
                        if (!result.Succeeded)
                            ModelState.AddModelError("message", "Cannot remove user from role.");

                    }
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction("Index", "Role");
            else
                return await Update(request.RoleId);
        }
    }
}
