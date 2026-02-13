using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Expctions
{
    public class BasketNotFoundExcepions(string id) : NotFoundException($"Product With Id {id} Not Found!")
    {

    }
}
