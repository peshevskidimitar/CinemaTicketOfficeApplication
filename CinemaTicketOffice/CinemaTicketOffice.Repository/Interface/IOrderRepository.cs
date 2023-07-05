using CinemaTicketOffice.Domain.Entity;
using CinemaTicketOffice.Domain.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Repository.Interface
{
    public interface IOrderRepository
    {
        public List<Order> GetAllOrders();
        public Order GetOrderDetails(BaseEntity model);
    }
}
