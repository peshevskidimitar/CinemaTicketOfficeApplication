using CinemaTicketOffice.Domain.Entity;
using CinemaTicketOffice.Domain.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Domain.Relation
{
    public class TicketInOrder : BaseEntity
    {
        public Guid TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int Quantity { get; set; }
    }
}
