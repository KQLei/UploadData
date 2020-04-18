using EF.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EF.Data
{
    public class Repository<T> where T : BaseEntity
    {
        private readonly EFDbContext _context;

        private DbSet<T> _entities;

        private string _errorMessage = string.Empty;

        public Repository(EFDbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public T GetById(object id)
        {
            return _entities.Find(id);
        }

        public void Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _entities.Add(entity);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                throw new Exception(_errorMessage);
            }
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }

        public virtual IQueryable<T> Table => _entities;

    }
}
