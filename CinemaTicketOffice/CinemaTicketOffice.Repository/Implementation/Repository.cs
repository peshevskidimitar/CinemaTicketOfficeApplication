using CinemaTicketOffice.Domain.Entity;
using CinemaTicketOffice.Repository.Data;
using CinemaTicketOffice.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaTicketOffice.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>()
                .AsEnumerable();
        }

        public T Get(Guid? id)
        {
            return _context.Set<T>()
                .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("The value of entity is null.");

            _context.Set<T>()
                .Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("The value of entity is null.");

            _context.Set<T>()
                .Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("The value of entity is null.");

            _context.Set<T>()
                .Remove(entity);
            _context.SaveChanges();
        }
    }
}
