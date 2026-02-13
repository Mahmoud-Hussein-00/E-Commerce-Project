using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstrations;
using Shared;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api / [Controller]")]
    public class AuthController(IServiceManger serviceManger) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var result = await serviceManger.AuthService.LoginAsync(loginDTO);
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var result = await serviceManger.AuthService.RegisterAsync(registerDTO);
            return Ok(result);
        }

        [HttpGet("{EmailExists}")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var result = await serviceManger.AuthService.CheckEmailExistsAsync(email);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManger.AuthService.GetCurrentUserAsync(email);
            return Ok(result);
        }

        [HttpGet("Address")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManger.AuthService.GetCurrentUserAddressAsync(email);
            return Ok(result);
        }

        [HttpPut("Address")]
        [Authorize]
        public async Task<IActionResult> UpdateCurrentUserAddress(AddressDTO address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManger.AuthService.UpdateCurrentUserAddressAsync(address, email);
            return Ok(result);
        }

    }
}
