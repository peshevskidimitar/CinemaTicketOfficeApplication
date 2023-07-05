using CinemaTicketOffice.Domain.Entity;
using CinemaTicketOffice.Domain.Models.Domain;
using CinemaTicketOffice.Repository.Interface;
using CinemaTicketOffice.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<Order> GetAllOrders()
        {
            return this._orderRepository.GetAllOrders();
        }

        public Order GetOrderDetails(BaseEntity model)
        {
            return this._orderRepository.GetOrderDetails(model);
        }
    }
}
