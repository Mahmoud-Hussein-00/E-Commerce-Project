using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstrations
{
    public interface IOrderService
    {
        Task<OrderResultDTO> GetOrderByIdAsync(Guid id);

        Task<IEnumerable<OrderResultDTO>> GetOrderByUserEmailAsync(string userEmail);

        Task<OrderResultDTO> CreateOrderAsync(OrderRequestDTO orderRequest, string userEmail);
    
        Task<IEnumerable<DeliveryMethodDTO>> GetAllDeliveryMethods();
    
    }
}
