namespace InsightCore.Application.DTO
{
    public record FoodProductDto(
        string? Barcode,
        string Name,
        string? ImageUrl,
        double? CaloriesKcal,
        double? ProteinsGram,
        double? CarbsGram,
        double? FatGram,
        double? FiberGram
    );
}
