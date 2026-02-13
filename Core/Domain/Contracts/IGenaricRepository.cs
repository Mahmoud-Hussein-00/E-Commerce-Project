using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenaricRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<int> CountAsync(ISpecifications<TEntity, TKey> spec);

        Task<IEnumerable<TEntity>> GetAllAsync(bool TrackCharnges = false);

        Task<TEntity?> GetAsync(TKey id);

        Task AddAsync(TEntity entity);
        
        void Update(TEntity entity);    

        void Delete(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> spec, bool TrackCharnges = false);

        Task<TEntity?> GetAsync(ISpecifications<TEntity, TKey> spec);

    }
}
