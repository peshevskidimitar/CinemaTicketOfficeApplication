using CinemaTicketOffice.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Security.Claims;

namespace CinemaTicketOffice.Web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(_shoppingCartService.GetShoppingCartInfo(userId));
        }

        public IActionResult DeleteFromShoppingCart(Guid id)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _shoppingCartService.DeleteTicketFromShoppingCart(userId, id);

            if (result)
                return RedirectToAction("Index", "ShoppingCart");
            else
                return RedirectToAction("Index", "ShoppingCart");
        }

        public Boolean Order()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _shoppingCartService.Order(userId);

            return result;
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = this._shoppingCartService.GetShoppingCartInfo(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "Cinema Ticket Office Application Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                var result = this.Order();

                if (result)
                    return RedirectToAction("Index", "ShoppingCart");
                else
                    return RedirectToAction("Index", "ShoppingCart");
            }

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
