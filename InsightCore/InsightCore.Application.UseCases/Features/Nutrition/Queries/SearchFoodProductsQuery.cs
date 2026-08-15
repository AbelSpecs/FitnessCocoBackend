using InsightCore.Application.DTO;
using MediatR;

namespace InsightCore.Application.UseCases.Features.Nutrition.Queries
{
    public record SearchFoodProductsQuery(string Term, int Page = 1, int PageSize = 10) : IRequest<PagedResult<FoodProductDto>>;
}
