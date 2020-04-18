using EF.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EF.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EFDbContext _context;

        private ConcurrentDictionary<string, object> _repositories;

        public UnitOfWork(EFDbContext eFDbContext)
        {
            _context = eFDbContext;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void RollBack()
        { }

        public Repository<T> Repository<T>() where T : BaseEntity
        {
            if (_repositories==null)
            {
                _repositories = new ConcurrentDictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var reposityInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                _repositories.TryAdd(type, reposityInstance);
            }

            return (Repository<T>)_repositories[type];

        }

    }
}
