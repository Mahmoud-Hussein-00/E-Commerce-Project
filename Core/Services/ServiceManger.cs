using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstrations;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManger(IUnitOfWork unitOfWork ,IMapper mapper,
        IBasketRepository basketRepository, ICacheRepository cacheRepository, 
        UserManager<AppUser> userManager,
        IOptions<JwtOptions> options,
        IConfiguration configuration
        ) : IServiceManger
    {
        public IProductService ProductService => new ProductService(unitOfWork, mapper);

        public IBasketService BasketService => new BasketServices(basketRepository, mapper);

        public ICasheService CasheService => new CasheService(cacheRepository);

        public IAuthService AuthService => new AuthService(userManager, options, mapper);

        public IOrderService OrderService => new OrderService(mapper, basketRepository, unitOfWork);

        public IPaymentService paymentService => new PaymentService(basketRepository, unitOfWork, mapper, configuration);
    }
}
