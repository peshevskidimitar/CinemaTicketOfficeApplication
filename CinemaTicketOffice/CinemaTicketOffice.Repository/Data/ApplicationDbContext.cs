using CinemaTicketOffice.Domain.Email;
using CinemaTicketOffice.Domain.Models.Domain;
using CinemaTicketOffice.Domain.Models.Identity;
using CinemaTicketOffice.Domain.Relation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaTicketOffice.Repository.Data
{
    public class ApplicationDbContext : IdentityDbContext<TicketOfficeUser>
    {
        public virtual DbSet<Ticket> TicketSet { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCartSet { get; set; }
        public virtual DbSet<TicketInShoppingCart> TicketInShoppingCartSet { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<TicketInOrder> TicketInOrderSet { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ticket>()
                .Property(ticket => ticket.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(shoppingCart => shoppingCart.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketInShoppingCart>()
                .Property(ticketInShoppingCart => ticketInShoppingCart.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Order>()
                .Property(order => order.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketInOrder>()
                .Property(ticketInOrder => ticketInOrder.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketInShoppingCart>()
                .HasOne(ticketInShoppingCart => ticketInShoppingCart.Ticket)
                .WithMany(ticket => ticket.TicketInShoppingCartCollection)
                .HasForeignKey(ticketInShoppingCart => ticketInShoppingCart.TicketId);

            builder.Entity<TicketInShoppingCart>()
                .HasOne(ticketInShoppingCart => ticketInShoppingCart.ShoppingCart)
                .WithMany(shoppingCart => shoppingCart.TicketInShoppingCartCollection)
                .HasForeignKey(ticketInShoppingCart => ticketInShoppingCart.ShoppingCartId);

            builder.Entity<ShoppingCart>()
                .HasOne(shoppingCart => shoppingCart.Owner)
                .WithOne(owner => owner.ShoppingCart)
                .HasForeignKey<ShoppingCart>(shoppingCart => shoppingCart.OwnerId);

            builder.Entity<TicketInOrder>()
                .HasOne(ticketInOrder => ticketInOrder.Ticket)
                .WithMany(ticket => ticket.TicketInOrderCollection)
                .HasForeignKey(ticketInOrder => ticketInOrder.TicketId);

            builder.Entity<TicketInOrder>()
                .HasOne(ticketInOrder => ticketInOrder.Order)
                .WithMany(order => order.TicketInOrderCollection)
                .HasForeignKey(ticketInOrder => ticketInOrder.OrderId);
        }
    }
}
