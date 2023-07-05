using CinemaTicketOffice.Domain.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Domain.Models.Identity
{
    public class TicketOfficeUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
