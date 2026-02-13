using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Expctions
{
    public class unAuthorizedException(string message="Invalid Email Or Password") 
        : Exception(message)
    {

    }
}
