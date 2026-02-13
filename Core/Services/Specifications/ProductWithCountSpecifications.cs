using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithCountSpecifications(ProductSpecificationsParamtars specParams) :
            base(p => string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search.ToLower())
            && (!specParams.brandId.HasValue || p.BrandId == specParams.brandId) &&
                      (!specParams.typeId.HasValue || p.TypeId == specParams.typeId))
        {
        }
    }
}
