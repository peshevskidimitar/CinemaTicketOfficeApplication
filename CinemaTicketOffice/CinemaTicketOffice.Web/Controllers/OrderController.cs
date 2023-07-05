using CinemaTicketOffice.Domain.Models.Domain;
using CinemaTicketOffice.Service.Interface;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CinemaTicketOffice.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Order> orders = _orderService.GetAllOrders().Where(x => x.UserId == userId).ToList();
            return View(orders);
        }

        public IActionResult CreateInvoice(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Order order = _orderService.GetAllOrders().Where(x => x.UserId == userId && x.Id == id).SingleOrDefault();

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates\\InvoiceTemplate.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", order.Id.ToString());
            document.Content.Replace("{{CustomerDetails}}", (order.User.FirstName + " " + order.User.LastName + " (" + order.User.Email + ")"));
            StringBuilder sb = new StringBuilder();
            foreach (var item in order.TicketInOrderCollection)
                sb.AppendLine("Ticket(s) for " + item.Ticket.MovieName + " with quantity of " + item.Quantity + " and price of $" + item.Ticket.Price + ". (Subtotal: $" + (item.Ticket.Price * item.Quantity) + ")");
            document.Content.Replace("{{Tickets}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", "$" + order.TotalPrice().ToString());

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "InvoiceOrder" + order.Id.ToString() + ".pdf");
        }
    }
}
