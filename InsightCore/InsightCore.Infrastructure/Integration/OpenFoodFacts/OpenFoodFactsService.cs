using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using InsightCore.Application.DTO;
using InsightCore.Application.Interface.Integration;
using Microsoft.Extensions.Logging;

namespace InsightCore.Infrastructure.Integration.OpenFoodFacts
{
    public class OpenFoodFactsService : IOpenFoodFactsService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };
        private readonly ILogger<OpenFoodFactsService> _logger;

        public OpenFoodFactsService(HttpClient http, ILogger<OpenFoodFactsService> logger)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<FoodProductDto?> GetProductByBarcodeAsync(string barcode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return null;
            var url = $"product/{Uri.EscapeDataString(barcode)}.json?lc=es";
            using var resp = await _http.GetAsync(url, cancellationToken);
            if (!resp.IsSuccessStatusCode) return null;
            using var stream = await resp.Content.ReadAsStreamAsync(cancellationToken);
            var wrapper = await JsonSerializer.DeserializeAsync<ProductResponse>(stream, _jsonOptions, cancellationToken);
            if (wrapper?.Product == null) return null;
            return MapProduct(wrapper.Product);
        }

        public async Task<PagedResult<FoodProductDto>> SearchProductsAsync(string searchTerm, int page, int pageSize, CancellationToken cancellationToken)
        {
            var fields = "code,product_name,product_name_es,product_name_nl,nutriments,image_front_url";

            // Use the classic CGI search endpoint which reliably honors search_terms and fields.
            var url = $"https://es.openfoodfacts.org/cgi/search.pl?search_terms={Uri.EscapeDataString(searchTerm ?? string.Empty)}&search_simple=1&action=process&json=1&page={page}&page_size={pageSize}&fields={fields}&lc=es";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
            if (!request.Headers.Contains("Accept-Language"))
                request.Headers.AcceptLanguage.ParseAdd("es-ES,es;q=0.9");

            _logger.LogDebug("OpenFoodFacts search URL: {Url}", url);

            using var resp = await _http.SendAsync(request, cancellationToken);
            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogWarning("OpenFoodFacts returned non-success status {Status} for URL {Url}", resp.StatusCode, url);
                return new PagedResult<FoodProductDto>(Array.Empty<FoodProductDto>(), 0, page, pageSize);
            }

            var content = await resp.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("OpenFoodFacts response for term '{Term}': {Content}", searchTerm, content);

            var sr = JsonSerializer.Deserialize<SearchResponse>(content, _jsonOptions);
            var items = (sr?.Products ?? Array.Empty<Product>())
                        .Select(MapProduct)
                        .Where(x => x != null)
                        .Cast<FoodProductDto>()
                        .ToArray();
            var total = sr?.Count ?? items.Length;
            return new PagedResult<FoodProductDto>(items, total, page, pageSize);
        }

        private static FoodProductDto? MapProduct(Product p)
        {
            if (p == null) return null;
            var name = p.ProductNameEs ?? p.ProductNameNl ?? p.ProductName ?? string.Empty;
            var nutr = p.Nutriments ?? new Nutriments();

            double? calories = nutr.EnergyKcal100g;
            if (calories == null && nutr.EnergyKj100g != null)
                calories = Math.Round(nutr.EnergyKj100g.Value / 4.184, 2);
            if (calories == null && nutr.Energy100g != null)
                calories = nutr.Energy100g;

            return new FoodProductDto(
                Barcode: p.Code,
                Name: name,
                ImageUrl: p.ImageFrontUrl,
                CaloriesKcal: calories,
                ProteinsGram: nutr.Proteins100g,
                CarbsGram: nutr.Carbohydrates100g,
                FatGram: nutr.Fat100g,
                FiberGram: nutr.Fiber100g
            );
        }

        private record ProductResponse([property: JsonPropertyName("product")] Product? Product);
        private record SearchResponse([property: JsonPropertyName("count")] int Count, [property: JsonPropertyName("products")] Product[]? Products);

        private class Product
        {
            [JsonPropertyName("code")] public string? Code { get; init; }
            [JsonPropertyName("product_name")] public string? ProductName { get; init; }
            [JsonPropertyName("product_name_es")] public string? ProductNameEs { get; init; }
            [JsonPropertyName("product_name_nl")] public string? ProductNameNl { get; init; }
            [JsonPropertyName("image_front_url")] public string? ImageFrontUrl { get; init; }
            [JsonPropertyName("nutriments")] public Nutriments? Nutriments { get; init; }
        }

        private class Nutriments
        {
            [JsonPropertyName("energy-kcal_100g")] public double? EnergyKcal100g { get; init; }
            [JsonPropertyName("energy-kj_100g")] public double? EnergyKj100g { get; init; }
            [JsonPropertyName("energy_100g")] public double? Energy100g { get; init; }
            [JsonPropertyName("proteins_100g")] public double? Proteins100g { get; init; }
            [JsonPropertyName("carbohydrates_100g")] public double? Carbohydrates100g { get; init; }
            [JsonPropertyName("fat_100g")] public double? Fat100g { get; init; }
            [JsonPropertyName("fiber_100g")] public double? Fiber100g { get; init; }
        }
    }
}
