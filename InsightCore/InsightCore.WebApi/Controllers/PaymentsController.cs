using System.Threading;
using System.Threading.Tasks;
using InsightCore.Application.DTO.Payments;
using InsightCore.Application.Interface.Payments;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPayPalService _payPal;

        public PaymentsController(IPayPalService payPal)
        {
            _payPal = payPal;
        }

        [HttpPost("paypal/create")]
        public async Task<IActionResult> CreatePayPalOrder([FromBody] PayPalCreateOrderRequest request, CancellationToken cancellationToken)
        {
            var res = await _payPal.CreateOrderAsync(request, cancellationToken);
            if (res == null) return StatusCode(502, "Error creating PayPal order");
            return Ok(res);
        }

        [HttpPost("paypal/capture/{orderId}")]
        public async Task<IActionResult> CapturePayPalOrder([FromRoute] string orderId, CancellationToken cancellationToken)
        {
            var res = await _payPal.CaptureOrderAsync(orderId, cancellationToken);
            if (res == null) return StatusCode(502, "Error capturing PayPal order");
            return Ok(res);
        }
    }
}
