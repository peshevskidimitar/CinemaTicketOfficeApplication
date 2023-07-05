using CinemaTicketOffice.Domain.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Domain.DTO.Domain
{
    public class AddToShoppingCartDTO
    {
        public Guid SelectedTicketId { get; set; }
        public Ticket SelectedTicket { get; set; }
        public int Quantity { get; set; }

    }
}
