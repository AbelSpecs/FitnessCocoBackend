using System.Threading;
using System.Threading.Tasks;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Integration;
using InsightCore.Application.UseCases.Features.Nutrition.Queries;
using MediatR;

namespace InsightCore.Application.UseCases.Features.Nutrition.Handlers
{
    public class SearchFoodProductsHandler : IRequestHandler<SearchFoodProductsQuery, PagedResult<FoodProductDto>>
    {
        private readonly IOpenFoodFactsService _svc;

        public SearchFoodProductsHandler(IOpenFoodFactsService svc)
        {
            _svc = svc;
        }

        public Task<PagedResult<FoodProductDto>> Handle(SearchFoodProductsQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
            return _svc.SearchProductsAsync(request.Term ?? string.Empty, page, pageSize, cancellationToken);
        }
    }
}
