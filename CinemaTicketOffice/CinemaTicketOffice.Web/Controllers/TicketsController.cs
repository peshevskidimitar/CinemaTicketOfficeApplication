using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaTicketOffice.Domain.Models.Domain;
using CinemaTicketOffice.Repository.Data;
using System.Diagnostics;
using System.Net.Sockets;
using CinemaTicketOffice.Domain.DTO.Domain;
using System.Security.Claims;
using CinemaTicketOffice.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;
using System.IO;

namespace CinemaTicketOffice.Web.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITicketService _ticketService;

        public TicketsController(ApplicationDbContext context, ITicketService ticketService)
        {
            _context = context;
            _ticketService = ticketService;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(DateTime? startTime, DateTime? endTime)
        {
            List<Ticket> tickets = await _context.TicketSet.ToListAsync();
            if (startTime != null && endTime != null)
                tickets = await _context.TicketSet
                    .Where(ticket => ticket.Time >= startTime && ticket.Time <= endTime)
                    .ToListAsync();

            return View(tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.TicketSet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieName,MovieGenre,MovieCoverImage,MovieDescription,Time,Price,Id")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.TicketSet.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MovieName,MovieGenre,MovieCoverImage,MovieDescription,Time,Price,Id")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.TicketSet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ticket = await _context.TicketSet.FindAsync(id);
            _context.TicketSet.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return _context.TicketSet.Any(e => e.Id == id);
        }

        public IActionResult AddTicketToCart(Guid id)
        {
            return View(_ticketService.GetShoppingCartInfo(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTicketToCart(AddToShoppingCartDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _ticketService.AddToShoppingCart(model, userId);
            if (result)
                return RedirectToAction("Index", "Tickets");
           
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public FileContentResult ExportTickets(string genre)
        {
            List<Ticket> tickets = _ticketService.GetAllTickets();
            if (!string.IsNullOrEmpty(genre))
                tickets = tickets
                    .Where(ticket => ticket.MovieGenre == genre)
                    .ToList();

            string fileName = "Tickets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workBook.Worksheets.Add("Tickets");

                worksheet.Cell(1, 1).Value = "Ticket Id";
                worksheet.Cell(1, 2).Value = "Movie Name";
                worksheet.Cell(1, 3).Value = "Movie Genre";
                worksheet.Cell(1, 4).Value = "Movie Cover Image";
                worksheet.Cell(1, 5).Value = "Movie Description";
                worksheet.Cell(1, 6).Value = "Time";
                worksheet.Cell(1, 7).Value = "Price";

                for (int i = 1; i <= tickets.Count(); i++)
                {
                    var ticket = tickets[i - 1];

                    worksheet.Cell(i + 1, 1).Value = ticket.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = ticket.MovieName.ToString();
                    worksheet.Cell(i + 1, 3).Value = ticket.MovieGenre.ToString();
                    worksheet.Cell(i + 1, 4).Value = ticket.MovieCoverImage.ToString();
                    worksheet.Cell(i + 1, 5).Value = ticket.MovieDescription.ToString();
                    worksheet.Cell(i + 1, 6).Value = ticket.Time.ToString();
                    worksheet.Cell(i + 1, 7).Value = "$" + ticket.Price.ToString();
                }

                using (var stream = new MemoryStream())
                {
                    workBook.SaveAs(stream);

                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }
        }
    }
}
