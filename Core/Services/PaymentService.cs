using AutoMapper;
using Domain.Contracts;
using Domain.Expctions;
using Domain.Models;
using Domain.Models.OrderModels;
using Microsoft.Extensions.Configuration;
using Services.Abstrations;
using Shared;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderProduct = Domain.Models.Product;

namespace Services
{
    public class PaymentService(
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IConfiguration configuration) : IPaymentService
    {
        public async Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);
            if (basket is null) throw new BasketNotFoundExcepions(basketId);

            foreach (var item in basket.Items)
            {
                var Product = await unitOfWork.GetRepository<OrderProduct, int>().GetAsync(item.Id);
                if(Product is null) throw new ProductNotFountException(item.Id);
                item.Price = Product.Price;
            }

            if (!basket.DeliveryMethodId.HasValue) throw new Exception("Invalid Delivery Method Id !!");

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
            if(deliveryMethod is null) throw new DeliveryMethodNotFoundExcpetion(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Cost;

            var amount = (long) (basket.Items.Sum(I => I.Price * I.Quantity) + basket.ShippingPrice) * 100;

            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

            var service = new PaymentIntentService();

            if(string.IsNullOrEmpty(basket.PaymentIntentID))
            {
                var CreateOptions = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() {"Card"}
                };

                var PaymentIntent = await service.CreateAsync(CreateOptions);
                basket.PaymentIntentID = PaymentIntent.Id;
                basket.ClientSecret = PaymentIntent.ClientSecret;

            }
            else
            {
                var UpdateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = amount,
                };

                var PaymentIntent = await service.UpdateAsync(basket.PaymentIntentID , UpdateOptions);
            }

            await basketRepository.UpdateBasketAsync(basket);
            var result = mapper.Map<BasketDTO>(basket);
            return result;

        }
    }
}
