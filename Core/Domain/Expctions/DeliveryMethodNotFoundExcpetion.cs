using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Expctions
{
    public class DeliveryMethodNotFoundExcpetion(int id) 
        : NotFoundException($"Delivary Method With Id {id} Is Not Found !")
    {
    }
}
