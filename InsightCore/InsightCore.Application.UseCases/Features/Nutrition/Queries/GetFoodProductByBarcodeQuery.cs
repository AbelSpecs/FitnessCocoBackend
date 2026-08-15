using InsightCore.Application.DTO;
using MediatR;

namespace InsightCore.Application.UseCases.Features.Nutrition.Queries
{
    public record GetFoodProductByBarcodeQuery(string Barcode) : IRequest<FoodProductDto?>;
}
