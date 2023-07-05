using CinemaTicketOffice.Domain.Entity;
using CinemaTicketOffice.Domain.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Domain.Relation
{
    public class TicketInShoppingCart : BaseEntity
    {
        public Guid TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public Guid ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }

        public int Quantity { get; set; }
    }
}
