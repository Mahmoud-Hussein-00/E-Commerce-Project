using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderModels
{
    public class OrderResultDTO
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public AddressDTO ShipingAddress { get; set; }
        public ICollection<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
        public string DeliveryMethod { get; set; }
        public string PaymentStatus { get; set; }
        public decimal SubTotal { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public string PaymentIntentId { get; set; } = string.Empty;
        public decimal Total { get; set; }

    }
}
