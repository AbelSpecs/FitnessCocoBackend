using System.Threading;
using System.Threading.Tasks;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Integration;
using InsightCore.Application.UseCases.Features.Nutrition.Queries;
using MediatR;

namespace InsightCore.Application.UseCases.Features.Nutrition.Handlers
{
    public class GetFoodProductByBarcodeHandler : IRequestHandler<GetFoodProductByBarcodeQuery, FoodProductDto?>
    {
        private readonly IOpenFoodFactsService _svc;

        public GetFoodProductByBarcodeHandler(IOpenFoodFactsService svc)
        {
            _svc = svc;
        }

        public Task<FoodProductDto?> Handle(GetFoodProductByBarcodeQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Barcode)) return Task.FromResult<FoodProductDto?>(null);
            return _svc.GetProductByBarcodeAsync(request.Barcode, cancellationToken);
        }
    }
}
