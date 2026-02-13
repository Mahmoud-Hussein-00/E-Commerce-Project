using Microsoft.AspNetCore.Mvc;
using Services.Abstrations;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api / [Controller]")]
    public class BasketController(IServiceManger serviceManger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasketById(string id)
        {
            var result = await serviceManger.BasketService.GetBasketAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBasket(BasketDTO basketDTO)
        {
            var result = await serviceManger.BasketService.UpdateBasketAsync(basketDTO);
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            var result = serviceManger.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }

    }
}
