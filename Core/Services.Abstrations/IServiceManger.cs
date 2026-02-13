using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstrations
{
    public interface IServiceManger
    {
        IProductService ProductService { get; }
        IBasketService BasketService { get; }
        ICasheService CasheService { get; }
        IAuthService AuthService { get; }
        IOrderService OrderService { get; }
        IPaymentService paymentService { get; }
    }
}
