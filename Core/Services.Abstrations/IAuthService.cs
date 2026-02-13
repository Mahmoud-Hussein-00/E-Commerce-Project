using Shared;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstrations
{
    public interface IAuthService
    {
        Task<UserResultDto> LoginAsync(LoginDTO LoginDto);
        Task<UserResultDto> RegisterAsync(RegisterDTO RegisterDto);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<UserResultDto> GetCurrentUserAsync(string email);
        Task<AddressDTO> GetCurrentUserAddressAsync(string email);
        Task<AddressDTO> UpdateCurrentUserAddressAsync(AddressDTO address, string email);
    }
}
