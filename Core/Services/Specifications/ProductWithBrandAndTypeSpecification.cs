using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecifications<Product, int>
    {
        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }

        public ProductWithBrandAndTypeSpecification(ProductSpecificationsParamtars specParams) :
            base(p => (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search.ToLower())
            && !specParams.brandId.HasValue || p.BrandId == specParams.brandId) &&
                      (!specParams.typeId.HasValue || p.TypeId == specParams.typeId))
        {
            ApplyIncludes();

            ApplySorting(specParams.sort);
          
            ApplayPagination(specParams.pageIndex, specParams.pageSize);

        }

        private void ApplyIncludes()
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDescening(P => P.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescening(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }

        }
    }
}
