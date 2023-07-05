using CinemaTicketOffice.Domain.DTO.Domain;
using CinemaTicketOffice.Domain.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Service.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();
        Ticket GetDetailsForTicket(Guid? id);
        void CreateNewTicket(Ticket ticket);
        void UpdateExistingTicket(Ticket ticket);
        AddToShoppingCartDTO GetShoppingCartInfo(Guid? id);
        void DeleteTicket(Guid id);
        bool AddToShoppingCart(AddToShoppingCartDTO item, string userID);
    }
}
