using Microsoft.Extensions.Logging;
using ProductSales.Application.Contracts;
using ProductSales.Application.DTOs;
using ProductSales.Application.Models;
using ProductSales.Domain.Entities;

namespace ProductSales.Application.Services;

public class ProductDashboardService : IProductDashboardService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductDashboardService> _logger;

    public ProductDashboardService( IProductRepository repository, ILogger<ProductDashboardService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<DashboardDto> GetAsync(CancellationToken cancellationToken)
    {
        // Get all products
        var products = await _repository.GetProductsAsync(cancellationToken);

        // Load sales for each product
        var productSales = await GetProductSalesAsync(products, cancellationToken);

        // Create sales summaries
        var summaries = productSales
            .Select(CreateProductSummary)
            .OrderByDescending(x => x.Revenue)
            .ToList();

        // Map products to DTOs
        var productDtos = products
            .Select(MapProduct)
            .ToList();

        // Calculate dashboard totals
        var totalTransactions = summaries.Sum(x => x.TransactionCount);

        var totalQuantitySold = summaries.Sum(x => x.QuantitySold);

        var totalRevenue = summaries.Sum(x => x.Revenue);

        var topSellingProduct = summaries
            .Where(x => x.QuantitySold > 0)
            .OrderByDescending(x => x.QuantitySold)
            .Select(x => x.Description)
            .FirstOrDefault();

        // Build dashboard response
        return new DashboardDto(
            productDtos,
            summaries,
            products.Count,
            totalTransactions,
            totalQuantitySold,
            totalRevenue,
            topSellingProduct);
    }

    private async Task<List<ProductSalesData>> GetProductSalesAsync(IReadOnlyList<Product> products, CancellationToken cancellationToken)
    {
        var tasks = products.Select(async product =>
        {
            var sales = await _repository.GetProductSalesAsync(
                product.Id,
                cancellationToken);

            return new ProductSalesData
            {
                Product = product,
                Sales = sales
            };
        });

        var result = await Task.WhenAll(tasks);

        return result.ToList();
    }

    private ProductSalesSummaryDto CreateProductSummary(ProductSalesData productSales)
    {

        var invalidSales = productSales.Sales
       .Where(x =>
           x.SaleQty < 0 ||
           x.SalePrice < 0)
       .ToList();

        if (invalidSales.Any())
        {
            _logger.LogWarning(
                "{Count} invalid sales records found for product {ProductId} ({ProductName})",
                invalidSales.Count,
                productSales.Product.Id,
                productSales.Product.Description);
        }

        var validSales = productSales.Sales
            .Where(x =>
                x.SaleQty >= 0 &&
                x.SalePrice >= 0)
            .ToList();

        var transactionCount = validSales.Count;

        var quantitySold = validSales.Sum(x => x.SaleQty);

        var revenue = validSales.Sum(x => x.Revenue);

        var averageSalePrice =
            transactionCount > 0
                ? validSales.Average(x => x.SalePrice)
                : 0;

        return new ProductSalesSummaryDto(
            productSales.Product.Id,
            productSales.Product.Description,
            productSales.Product.Category,
            transactionCount,
            quantitySold,
            revenue,
            averageSalePrice);
    }

    private static ProductDto MapProduct(Product product)
    {
        return new ProductDto(
            product.Id,
            product.Description,
            product.SalePrice,
            product.Category,
            product.Image);
    }
}
