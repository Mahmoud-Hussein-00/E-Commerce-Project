using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstrations
{
    public interface ICasheService
    {
        Task SetCasheValueAsync(string key, object value, TimeSpan duration);
        Task<string?> GetCasheValueAsync(string key);
    }
}
