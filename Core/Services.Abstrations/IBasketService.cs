using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstrations
{
    public interface IBasketService
    {
        Task<BasketDTO?> GetBasketAsync(string id);
        Task<BasketDTO?> UpdateBasketAsync(BasketDTO basketDTO);
        Task<bool> DeleteBasketAsync(string id);
    }
}
