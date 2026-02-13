using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Expctions
{
    public class OrderNotFoundExceprion(Guid id) 
        : NotFoundException($"Order With Id {id} Is Not Found !")
    {
    }
}
