using CinemaTicketOffice.Domain.DTO.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDTO GetShoppingCartInfo(string userId);
        bool DeleteTicketFromShoppingCart(string userId, Guid ticketId);
        bool Order(string userId);
    }
}
