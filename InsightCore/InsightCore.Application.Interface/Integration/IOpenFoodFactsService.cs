using System.Threading;
using System.Threading.Tasks;
using InsightCore.Application.DTO;

namespace InsightCore.Application.Interface.Integration
{
    public interface IOpenFoodFactsService
    {
        Task<FoodProductDto?> GetProductByBarcodeAsync(string barcode, CancellationToken cancellationToken);
        Task<PagedResult<FoodProductDto>> SearchProductsAsync(string searchTerm, int page, int pageSize, CancellationToken cancellationToken);
    }
}
