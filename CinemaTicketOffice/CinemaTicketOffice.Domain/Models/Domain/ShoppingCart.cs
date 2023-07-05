using CinemaTicketOffice.Domain.Entity;
using CinemaTicketOffice.Domain.Models.Identity;
using CinemaTicketOffice.Domain.Relation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Domain.Models.Domain
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public TicketOfficeUser Owner { get; set; }

        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCartCollection { get; set; }
    }
}
