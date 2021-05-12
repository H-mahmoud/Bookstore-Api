using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Models.Repository
{
    public class GenericRepository <T> : IDataRepository<T> where T : class
    {
        public BookStoreContext _context { get; }
        public DbSet<T> _table { get; }

        public GenericRepository (BookStoreContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public IEnumerable<T> FindAll()
        {
            return _table
                .ToList();
        }

        public T FindById(string Id)
        {
            return _table.Find(Id);
        }

        public void Create(T entity)
        {
            _table.Add(entity);
        }

        public bool Update(T entity, string Id)
        {
            T item = this.FindById(Id);

            if (item == null)
                return false;

            _context.Entry(item).CurrentValues.SetValues(entity);
            return true;
        }

        public bool Delete(string Id)
        {
            T entity = this.FindById(Id);

            if (entity == null)
                return false;

            _table.Remove(entity);
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
