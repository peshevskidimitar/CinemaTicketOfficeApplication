using CinemaTicketOffice.Domain.Entity;
using CinemaTicketOffice.Domain.Models.Domain;
using CinemaTicketOffice.Repository.Data;
using CinemaTicketOffice.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
        public List<Order> GetAllOrders()
        {
            return _context.Set<Order>()
                .Include(order => order.User)
                .Include(order => order.TicketInOrderCollection)
                .Include("TicketInOrderCollection.Ticket")
                .ToListAsync()
                .Result;
        }

        public Order GetOrderDetails(BaseEntity model)
        {
            return _context.Set<Order>()
               .Include(order => order.User)
               .Include(order => order.TicketInOrderCollection)
               .Include("TicketInOrderCollection.Ticket")
               .SingleOrDefaultAsync(z => z.Id == model.Id)
               .Result;
        }
    }
}
