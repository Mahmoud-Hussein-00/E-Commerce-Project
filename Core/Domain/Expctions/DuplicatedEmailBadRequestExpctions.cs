using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Expctions
{
    public class DuplicatedEmailBadRequestExpctions(string Email) 
        : BadRequestException($"There are Another User Use This Email{Email}")
    {
    }
}
