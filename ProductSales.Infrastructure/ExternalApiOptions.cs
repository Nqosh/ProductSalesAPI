namespace ProductSales.Infrastructure;

public sealed class ExternalApiOptions
{
    public const string Section = "ExternalApi";

    public required string BaseUrl { get; init; }

    public string ProductsPath { get; init; } = "products";

    public string ProductSalesPath { get; init; } = "product-sales";

    public int CacheSeconds { get; init; } = 30;

}