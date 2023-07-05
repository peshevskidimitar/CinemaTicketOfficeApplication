using CinemaTicketOffice.Domain.Relation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Domain.DTO.Domain
{
    public class ShoppingCartDTO
    {
        public List<TicketInShoppingCart> TicketInShoppingCartList { get; set; }
        public double TotalPrice { get; set; }
    }
}
