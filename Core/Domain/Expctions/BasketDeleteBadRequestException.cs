using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Expctions
{
    public class BasketDeleteBadRequestException() :
        BadRequestException($"Invalid Operation When Delete Basket !")
    {
    }
}
