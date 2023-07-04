using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Domain.Models.Identity
{
    public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<TicketOfficeUser> Members { get; set; }
        public IEnumerable<TicketOfficeUser> NonMembers { get; set; }
    }
}
