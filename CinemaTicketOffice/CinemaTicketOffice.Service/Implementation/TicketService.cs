using CinemaTicketOffice.Domain.DTO.Domain;
using CinemaTicketOffice.Domain.Models.Domain;
using CinemaTicketOffice.Domain.Relation;
using CinemaTicketOffice.Repository.Interface;
using CinemaTicketOffice.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaTicketOffice.Service.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<TicketInShoppingCart> _ticketInShoppingCartRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(IRepository<Ticket> ticketRepository, IRepository<TicketInShoppingCart> ticketInShoppingCartRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _ticketInShoppingCartRepository = ticketInShoppingCartRepository;
            _userRepository = userRepository;
        }

        public bool AddToShoppingCart(AddToShoppingCartDTO item, string userId)
        {
            var user = _userRepository.GetById(userId);
            var shoppingCart = user.ShoppingCart;
            if (item.SelectedTicketId == null || shoppingCart == null)
                return false;

            var ticket = _ticketRepository.Get(item.SelectedTicketId);
            if (ticket == null)
                return false;

            TicketInShoppingCart itemToAdd = new TicketInShoppingCart()
            {
                Ticket = ticket,
                TicketId = ticket.Id,
                ShoppingCart = shoppingCart,
                ShoppingCartId = shoppingCart.Id,
                Quantity = item.Quantity,
            };

            var existing = shoppingCart.TicketInShoppingCartCollection
                .Where(x => x.ShoppingCartId == shoppingCart.Id && x.TicketId == itemToAdd.TicketId)
                .FirstOrDefault();
            if (existing != null)
            {
                existing.Quantity += itemToAdd.Quantity;
                _ticketInShoppingCartRepository.Update(existing);
            }
            else
                _ticketInShoppingCartRepository.Insert(itemToAdd);

            return true;
        }

        public void CreateNewTicket(Ticket ticket)
        {
            _ticketRepository.Insert(ticket);
        }

        public void DeleteTicket(Guid id)
        {
            var ticket = _ticketRepository.Get(id);
            _ticketRepository.Delete(ticket);
        }

        public List<Ticket> GetAllTickets()
        {
            return _ticketRepository.GetAll().ToList();
        }

        public Ticket GetDetailsForTicket(Guid? id)
        {
            return _ticketRepository.Get(id);
        }

        public AddToShoppingCartDTO GetShoppingCartInfo(Guid? id)
        {
            var ticket = _ticketRepository.Get(id);
            AddToShoppingCartDTO model = new AddToShoppingCartDTO()
            {
                SelectedTicket = ticket,
                SelectedTicketId = ticket.Id,
                Quantity = 1
            };

            return model;
        }

        public void UpdateExistingTicket(Ticket ticket)
        {
            _ticketRepository.Update(ticket);
        }
    }
}
