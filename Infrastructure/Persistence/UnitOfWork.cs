using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Persistence.Data;
using Persistence.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        private readonly ConcurrentDictionary<string, object> _repositories;
        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<string, object>();
        }

        //public IGenaricRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        //{
        //    var typeName = typeof(TEntity).Name;
        //    if (!_repositories.ContainsKey(typeName))
        //    {
        //        var reposiroty = new GenaricReposatory<TEntity, TKey>(_context);
        //        _repositories.Add(typeName, reposiroty);
        //    }
        //    return (IGenaricRepository<TEntity, TKey>)_repositories[typeName];
        //}


        public IGenaricRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        => (IGenaricRepository<TEntity, TKey>) _repositories.GetOrAdd(typeof(TEntity).Name, new GenaricReposatory<TEntity, TKey>(_context));

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
