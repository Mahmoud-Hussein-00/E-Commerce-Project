using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstrations;
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
    [Authorize]
    public class OrdersController(IServiceManger serviceManger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult>CreateOrder(OrderRequestDTO request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManger.OrderService.CreateOrderAsync(request, email);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManger.OrderService.GetOrderByUserEmailAsync(email);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var result = await serviceManger.OrderService.GetOrderByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<IActionResult> GetAllDeliveryMethods()
        {
            var result = await serviceManger.OrderService.GetAllDeliveryMethods();
            return Ok(result);
        }

    }
}
