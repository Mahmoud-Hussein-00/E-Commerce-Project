using Microsoft.AspNetCore.Mvc;
using Services.Abstrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController(IServiceManger serviceManger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdatePaymentIntent(string basketId)
        {
            var result = await serviceManger.paymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(result);
        }
    }
}
