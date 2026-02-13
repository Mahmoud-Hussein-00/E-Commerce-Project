using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenaricReposatory<TEntity, TKey> : IGenaricRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _Context;

        public GenaricReposatory(StoreDbContext context)
        {
            _Context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool TrackCharnges = false)
        {
            if(typeof(TEntity) == typeof(Product))
            {
                return TrackCharnges?
                    await _Context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync() as IEnumerable<TEntity>
               :
                    await _Context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).AsNoTracking().ToListAsync() as IEnumerable<TEntity>;
            }


            if (TrackCharnges)
            {
               return await _Context.Set<TEntity>().ToListAsync();
            }
            else
            {
               return await _Context.Set<TEntity>().AsNoTracking().ToListAsync();
            }
        }

        public async Task<TEntity?> GetAsync(TKey id)
        {
            if (typeof(TEntity) == typeof(Product))
            { 
                return await _Context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).FirstOrDefaultAsync(P => P.Id == id as int? ) as TEntity ;
            }
                return await _Context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _Context.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _Context.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _Context.Remove(entity);
        }


        private IQueryable<TEntity> AddSpecifications(ISpecifications<TEntity, TKey> spec)
        {
            return SpecificationEvaluator.GetQuery(_Context.Set<TEntity>(), spec);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> spec , bool TrackCharnges = false)
        {
            return await AddSpecifications(spec).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await AddSpecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await AddSpecifications(spec).CountAsync();
        }
    }
}
