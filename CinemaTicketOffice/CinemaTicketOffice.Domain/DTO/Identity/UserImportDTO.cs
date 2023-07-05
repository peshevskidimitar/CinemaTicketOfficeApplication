using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Domain.DTO.Identity
{
    public class UserImportDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
