using AutoMapper;
using Domain.Contracts;
using Domain.Expctions;
using Domain.Models;
using Domain.Models.Identity;
using Domain.Models.OrderModels;
using Services.Abstrations;
using Services.Specifications;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService(IMapper mapper,
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork) : IOrderService
    {


        public async Task<OrderResultDTO> CreateOrderAsync(OrderRequestDTO orderRequest, string userEmail)
        {
            var address = mapper.Map<Domain.Models.OrderModels.Address>(orderRequest.ShipToAddress);
             
            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId);
            if(basket is null) throw new BasketNotFoundExcepions(orderRequest.BasketId);

            var orderItims = new List<OrderItem>();
            foreach(var item in basket.Items)
            {
                var Product = await unitOfWork.GetRepository<Product, int>().GetAsync(item.Id);
                if(Product is null) throw new ProductNotFountException(item.Id);
                var orderItem = new OrderItem(new ProductInOrderItem(Product.Id, Product.Name, Product.PictureUrl), item.Quantity, Product.Price);
                orderItims.Add(orderItem);
            }

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(orderRequest.DeliveryMethodId);
            if (deliveryMethod is null) throw new DeliveryMethodNotFoundExcpetion(orderRequest.DeliveryMethodId);

            var subTotal = orderItims.Sum(i => i.Price * i.Quantity);

            var spec = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentID);

            var existsOrder = await unitOfWork.GetRepository<Order, Guid>().GetAsync(spec);
            if(existsOrder is not null)
                unitOfWork.GetRepository<Order,Guid>().Delete(existsOrder);
            
            var order = new Order(userEmail, address, orderItims, deliveryMethod.Id, subTotal, basket.PaymentIntentID);
            await unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            var count = await unitOfWork.SaveChangesAsync();
            if (count == 0) throw new OrderCreateBadRequestException();
            var result = mapper.Map<OrderResultDTO>(order);

            return result;

        }

        public async Task<IEnumerable<DeliveryMethodDTO>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<DeliveryMethodDTO>>(deliveryMethods);
            return result;
        }

        public async Task<OrderResultDTO> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecifications(id);

            var order = await unitOfWork.GetRepository<Order, Guid>().GetAsync(spec);
            if (order is null) throw new OrderNotFoundExceprion(id);

            var result = mapper.Map<OrderResultDTO>(order);
            return result;
        }

        public async Task<IEnumerable<OrderResultDTO>> GetOrderByUserEmailAsync(string userEmail)
        {
            var spec = new OrderSpecifications(userEmail);

            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);

            var result = mapper.Map<IEnumerable<OrderResultDTO>>(orders);
            return result;

        }

    }
}
