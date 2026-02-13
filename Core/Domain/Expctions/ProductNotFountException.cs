using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Expctions
{
    public class ProductNotFountException(int id) : NotFoundException($"Product With Id {id} Not Found !")
    {
    }
}
