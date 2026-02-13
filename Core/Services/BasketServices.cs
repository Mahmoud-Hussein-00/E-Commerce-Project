using AutoMapper;
using Domain.Contracts;
using Domain.Expctions;
using Domain.Models;
using Services.Abstrations;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BasketServices(IBasketRepository basketRepository, IMapper mapper) : IBasketService
    {
        

        public async Task<BasketDTO?> GetBasketAsync(string id)
        {
            var basket = await basketRepository.GetBasketAsync(id);
            if (basket is null) throw new BasketNotFoundExcepions(id);
            var result = mapper.Map<BasketDTO>(basket);
            return result;
        }

        public async Task<BasketDTO?> UpdateBasketAsync(BasketDTO basketDTO)
        {
            var basket = mapper.Map<CustomerBasket>(basketDTO);
            basket = await basketRepository.UpdateBasketAsync(basket);
            if (basket is null) throw new BasketCreateOrUpdateBadRequestExcpetion();
            var result = mapper.Map<BasketDTO>(basket);
            return result;
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            var flag = await basketRepository.DeleteBasketAsync(id);
            if(flag == false) throw new BasketDeleteBadRequestException();
            return flag;
        }
    }
}
