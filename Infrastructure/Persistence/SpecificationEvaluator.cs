using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Persistence;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    static class SpecificationEvaluator
    {
      
        public static IQueryable<TEntity> GetQuery<TEntity, Tkey>
            (IQueryable<TEntity> inputQuery , 
            ISpecifications<TEntity, Tkey> spec)
            where TEntity : BaseEntity<Tkey>
        {
            var query = inputQuery;

            if (spec.Criteria is not null)
            { query = query.Where(spec.Criteria); }


            if(spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if(spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }


            if (spec.IsPagination)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }


            query = spec.IncludeExpressions.Aggregate(query, (currentQuery, includeExpression)
                => currentQuery.Include(includeExpression));

            return query;

        }
    

    }
}

