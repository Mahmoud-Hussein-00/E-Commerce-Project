using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Expctions
{
    public class OrderCreateBadRequestException() 
        : BadRequestException("Invalid Opration When Create Order")
    {
    }
}
