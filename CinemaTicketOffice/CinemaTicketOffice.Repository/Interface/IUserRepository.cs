using CinemaTicketOffice.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<TicketOfficeUser> GetAll();
        TicketOfficeUser GetById(string id);
        void Add(TicketOfficeUser user);
        void Edit(TicketOfficeUser user);
        void Delete(TicketOfficeUser user);
    }
}
