using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrderModels
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
        }

        public Order(string userEmail, Address shipingAddress, ICollection<OrderItem> orderItems, int? deliveryMethodId, decimal subTotal, string paymentIntentId)
        {
            Id = Guid.NewGuid();
            UserEmail = userEmail;
            ShipingAddress = shipingAddress;
            OrderItems = orderItems;
            DeliveryMethodId = deliveryMethodId;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string UserEmail { get ; set; }
        public Address ShipingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public DeliveryMethod DeliveryMethod { get; set; }
        public int? DeliveryMethodId { get; set; }
        public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;
        public decimal SubTotal { get; set; }
        public DateTimeOffset OrderDate {  get; set; } = DateTimeOffset.Now;
        public string PaymentIntentId { get; set; }


    }
}
