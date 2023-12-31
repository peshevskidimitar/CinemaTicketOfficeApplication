﻿using CinemaTicketOffice.Domain.Entity;
using CinemaTicketOffice.Domain.Models.Identity;
using CinemaTicketOffice.Domain.Relation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Domain.Models.Domain
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public TicketOfficeUser User { get; set; }

        public virtual ICollection<TicketInOrder> TicketInOrderCollection { get; set; }

        public double TotalPrice()
        {
            double totalPrice = 0;
            foreach (var item in TicketInOrderCollection)
                totalPrice += item.Ticket.Price * item.Quantity;

            return totalPrice;
        }
    }
}
