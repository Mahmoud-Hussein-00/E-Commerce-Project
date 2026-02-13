using AutoMapper;
using Domain.Expctions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Services.Abstrations;
using Shared;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService(UserManager<AppUser> userManager, IOptions<JwtOptions> options, IMapper mapper)
        : IAuthService
    {

        public async Task<UserResultDto> LoginAsync(LoginDTO LoginDto)
        {
            var user = await userManager.FindByEmailAsync(LoginDto.Email);
            if (user is null) { throw new unAuthorizedException(); }
            var flag = await userManager.CheckPasswordAsync(user, LoginDto.Password);
            if (!flag) { throw new unAuthorizedException(); }
            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user)
            };
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDTO RegisterDto)
        {

            if(await CheckEmailExistsAsync(RegisterDto.Email))
            {
                throw new DuplicatedEmailBadRequestExpctions(RegisterDto.Email);
            }

            var user = new AppUser()
            {
                DisplayName = RegisterDto.DisplayName,
                Email = RegisterDto.Email,
                UserName = RegisterDto.UserName,
                PhoneNumber = RegisterDto.PhoneNumber,
            };
            var Result = await userManager.CreateAsync(user, RegisterDto.Password);

            if (!Result.Succeeded)
            {
                var errors = Result.Errors.Select(error => error.Description);
                throw new ValidationException(errors);
            }

            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user)
            };
        }





        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<UserResultDto> GetCurrentUserAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) throw new UserNotFountExpction(email);
            return new UserResultDto()
            {
                DisplayName= user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtTokenAsync(user)
            };
        }

        public async Task<AddressDTO> GetCurrentUserAddressAsync(string email)
        {
            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);

            if (user is null) throw new UserNotFountExpction(email);
            var result = mapper.Map<AddressDTO>(user.Address);
            return result;
        }

        public async Task<AddressDTO> UpdateCurrentUserAddressAsync(AddressDTO address, string email)
        {
            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);

            if (user is null) throw new UserNotFountExpction(email);
            
            if(user.Address is not null)
            {
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.Street = address.Street;
                user.Address.City = address.City;
                user.Address.Country = address.Country;
            }
            else{
                var result = mapper.Map<Address>(address);
                user.Address = result;
            }

            await userManager.UpdateAsync(user);

            return address;
        }





        private async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            var JwtOptions = options.Value;

            var authClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role));
            }

            var SecretKey = new SymmetricSecurityKey
                (
                    Encoding.UTF8.GetBytes(JwtOptions.SecretKey)
                );

            var Token = new JwtSecurityToken
                (
                    issuer : JwtOptions.issuer,
                    audience : JwtOptions.Audience,
                    claims : authClaim,
                    expires: DateTime.UtcNow.AddDays(JwtOptions.DurationInDays),
                    signingCredentials:new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

    }
}
