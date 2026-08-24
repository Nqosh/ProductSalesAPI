using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ProductSales.Application.Contracts;
using ProductSales.Domain.Entities;

namespace ProductSales.Infrastructure;

public class LiveProductRepository(
    HttpClient httpClient,
    IMemoryCache cache,
    IOptions<ExternalApiOptions> options)
    : IProductRepository
{
    private readonly ExternalApiOptions _options =
        options.Value;

    public Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken ct)
    {
        return GetCachedAsync<IReadOnlyList<Product>>(
            "products",
            _options.ProductsPath,
            ct);
    }

    public Task<IReadOnlyList<ProductSale>> GetProductSalesAsync(int productId, CancellationToken ct)
    {
        return GetCachedAsync<IReadOnlyList<ProductSale>>(
            $"sales-{productId}",
            $"{_options.ProductSalesPath}?Id={productId}",
            ct);
    }

    private Task<T> GetCachedAsync<T>(string cacheKey,string endpoint, CancellationToken ct)
    {
        return cache.GetOrCreateAsync(
            cacheKey,
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromSeconds(
                        _options.CacheSeconds);

                return await FetchAsync<T>(
                    endpoint,
                    ct);
            })!;
    }

    private async Task<T> FetchAsync<T>(string endpoint, CancellationToken ct)
    {
        try
        {
            var result =
                await httpClient.GetFromJsonAsync<T>(
                    endpoint,
                    ct);

            return result ??
                throw new ExternalApiException(
                    $"Empty response from '{endpoint}'.");
        }
        catch (HttpRequestException ex)
        {
            throw new ExternalApiException(
                $"Failed calling '{endpoint}'.",
                ex);
        }
        catch (JsonException ex)
        {
            throw new ExternalApiException(
                $"Failed to deserialize response from '{endpoint}'.",
                ex);
        }
    }
}