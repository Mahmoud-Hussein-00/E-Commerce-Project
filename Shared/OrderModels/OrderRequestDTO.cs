using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderModels
{
    public class OrderRequestDTO
    {
        public string BasketId { get; set; }
        public AddressDTO ShipToAddress { get; set; }
        public int DeliveryMethodId { get ; set; }
    }
}
