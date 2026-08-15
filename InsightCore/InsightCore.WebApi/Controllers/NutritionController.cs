using System.Threading;
using System.Threading.Tasks;
using InsightCore.Application.DTO;
using InsightCore.Application.UseCases.Features.Nutrition.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InsightCore.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/nutrition/products")]
    public class NutritionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NutritionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetByBarcode([FromRoute] string barcode, CancellationToken cancellationToken)
        {
            var product = await _mediator.Send(new GetFoodProductByBarcodeQuery(barcode), cancellationToken);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new SearchFoodProductsQuery(term, page, pageSize), cancellationToken);
            return Ok(result);
        }
    }
}
