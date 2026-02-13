using Domain.Contracts;
using Services.Abstrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CasheService(ICacheRepository cacheRepository) : ICasheService
    {

        public async Task<string?> GetCasheValueAsync(string key)
        {
            var value = await cacheRepository.GetAsync(key);
            return value == null ? null : value;
        }

        public async Task SetCasheValueAsync(string key, object value, TimeSpan duration)
        {
            await cacheRepository.SetAsync(key, value, duration);
        }
    }
}
