using CinemaTicketOffice.Domain.Models.Identity;
using CinemaTicketOffice.Repository.Data;
using CinemaTicketOffice.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaTicketOffice.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TicketOfficeUser> GetAll()
        {
            return _context.Set<TicketOfficeUser>()
                .AsEnumerable();
        }

        public TicketOfficeUser GetById(string id)
        {
            return _context.Set<TicketOfficeUser>()
                .Include("ShoppingCart")
                .Include("ShoppingCart.TicketInShoppingCartCollection")
                .Include("ShoppingCart.TicketInShoppingCartCollection.Ticket")
                .SingleOrDefault(user => user.Id == id);
        }

        public void Add(TicketOfficeUser user)
        {
            if (user == null) 
                throw new ArgumentNullException("The value of user is null.");

            _context.Set<TicketOfficeUser>()
                .Add(user);
            _context.SaveChanges();
        }

        public void Edit(TicketOfficeUser user)
        {
            if (user == null)
                throw new ArgumentNullException("The value of user is null.");

            _context.Set<TicketOfficeUser>()
                .Update(user);
            _context.SaveChanges();
        }

        public void Delete(TicketOfficeUser user)
        {
            if (user == null)
                throw new ArgumentNullException("The value of user is null.");

            _context.Set<TicketOfficeUser>()
                .Remove(user);
            _context.SaveChanges();
        }
    }
}
