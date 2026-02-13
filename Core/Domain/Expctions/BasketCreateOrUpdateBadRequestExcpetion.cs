using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Expctions
{
    public class BasketCreateOrUpdateBadRequestExcpetion() :
        BadRequestException($"Invalid Operation When Create Or Update Basket !")
    {
    }
}
