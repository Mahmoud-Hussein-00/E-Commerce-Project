using Services.Abstrations;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using AutoMapper;
using Services.Specifications;
using Domain.Expctions;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWord, IMapper mapper) : IProductService
    {

        public async Task<PaginationResponse<ProductResultDTO>> GetAllProductAcync(ProductSpecificationsParamtars specParams)
        {
            var spec = new ProductWithBrandAndTypeSpecification(specParams);

            var Products = await unitOfWord.GetRepository<Product, int>().GetAllAsync(spec);

            var specCount = new ProductWithCountSpecifications(specParams); 
            var Count = await unitOfWord.GetRepository<Product, int>().CountAsync(specCount);

            var result = mapper.Map<IEnumerable<ProductResultDTO>>(Products);
            return new PaginationResponse<ProductResultDTO>(specParams.pageIndex,specParams.pageSize,Count,result);
        }
       

        public async Task<ProductResultDTO?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);

            var Product = await unitOfWord.GetRepository<Product, int>().GetAsync(spec);
            if (Product is null) throw new ProductNotFountException(id);

            var result = mapper.Map<ProductResultDTO>(Product);
            return result;
        }
        public async Task<IEnumerable<BrandResultDTO>> GetAllBrandsAsync()
        {
            var Brands = await unitOfWord.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandResultDTO>>(Brands);
            return result;
        }

        public async Task<IEnumerable<TypeResultDTO>> GetAllTypesAsync()
        {
            var Types = await unitOfWord.GetRepository<ProductType, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<TypeResultDTO>>(Types);
            return result;
        }

    }
}
