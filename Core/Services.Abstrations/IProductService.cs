using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstrations
{
    public interface IProductService
    {

        Task<PaginationResponse<ProductResultDTO>> GetAllProductAcync(ProductSpecificationsParamtars specParams);

        Task<ProductResultDTO?> GetProductByIdAsync(int id);

        Task<IEnumerable<BrandResultDTO>> GetAllBrandsAsync();

        Task<IEnumerable<TypeResultDTO>> GetAllTypesAsync();

    }
}
