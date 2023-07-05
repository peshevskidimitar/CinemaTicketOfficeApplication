using CinemaTicketOffice.Domain.DTO.Domain;
using CinemaTicketOffice.Domain.Email;
using CinemaTicketOffice.Domain.Models.Domain;
using CinemaTicketOffice.Domain.Relation;
using CinemaTicketOffice.Repository.Interface;
using CinemaTicketOffice.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace CinemaTicketOffice.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<TicketInOrder> _ticketInOrderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<EmailMessage> _emailMessageRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IRepository<Order> orderRepository, IRepository<TicketInOrder> ticketInOrderRepository, IUserRepository userRepository, IRepository<EmailMessage> emailMessageRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _orderRepository = orderRepository;
            _ticketInOrderRepository = ticketInOrderRepository;
            _userRepository = userRepository;
            _emailMessageRepository = emailMessageRepository;
        }

        public bool DeleteTicketFromShoppingCart(string userId, Guid ticketId)
        {
            if (string.IsNullOrEmpty(userId) || ticketId == null)
                return false;

            var loggedInUser = _userRepository.GetById(userId);
            var userShoppingCart = loggedInUser.ShoppingCart;
            var itemToDelete = userShoppingCart.TicketInShoppingCartCollection
                .Where(ticketInShoppingCart => ticketInShoppingCart.TicketId.Equals(ticketId))
                .FirstOrDefault();
            userShoppingCart.TicketInShoppingCartCollection.Remove(itemToDelete);
            _shoppingCartRepository.Update(userShoppingCart);

            return true;
        }

        public ShoppingCartDTO GetShoppingCartInfo(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;

            var loggedInUser = _userRepository.GetById(userId);
            var userShoppingCart = loggedInUser.ShoppingCart;
            var items = userShoppingCart.TicketInShoppingCartCollection.ToList();

            double totalPrice = 0.0;
            foreach (var item in items)
                totalPrice += item.Quantity * item.Ticket.Price;

            return new ShoppingCartDTO()
            {
                TicketInShoppingCartList = items,
                TotalPrice = totalPrice,
            };
        }

        public bool Order(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            var loggedInUser = _userRepository.GetById(userId);
            var userShoppingCart = loggedInUser.ShoppingCart;

            EmailMessage mail = new EmailMessage();
            mail.MailTo = loggedInUser.Email;
            mail.Subject = "Sucessfuly created order!";
            mail.Status = false;

            Order order = new Order()
            {
                User = loggedInUser,
                UserId = userId,
            };

            _orderRepository.Insert(order);

            List<TicketInOrder> ticketInOrders = new List<TicketInOrder>();

            var result = userShoppingCart.TicketInShoppingCartCollection
                .Select(item => new TicketInOrder()
                {
                    TicketId = item.TicketId,
                    Ticket = item.Ticket,
                    Order = order,
                    OrderId = order.Id,
                    Quantity = item.Quantity
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            sb.AppendLine("Your order is completed. The order conatins: ");

            for (int i = 1; i <= result.Count(); i++)
            {
                var currentItem = result[i - 1];
                totalPrice += currentItem.Quantity * currentItem.Ticket.Price;
                sb.AppendLine(i.ToString() + ". Ticket(s) for the movie " + currentItem.Ticket.MovieName + " with quantity of " + currentItem.Quantity + " and price of $" + currentItem.Ticket.Price + ".");
            }

            sb.AppendLine("Total price for your order is $" + totalPrice.ToString() + ".");

            mail.Content = sb.ToString();

            ticketInOrders.AddRange(result);

            foreach (var item in ticketInOrders)
                _ticketInOrderRepository.Insert(item);
            loggedInUser.ShoppingCart.TicketInShoppingCartCollection.Clear();

            _userRepository.Edit(loggedInUser);
            _emailMessageRepository.Insert(mail);

            return true;
        }
    }
}
