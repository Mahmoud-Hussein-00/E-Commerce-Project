using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Expctions
{
    public class UserNotFountExpction(string email) 
        : Exception($"User With Email {email} Not Found !")
    {
    }
}
