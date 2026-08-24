using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ProductSales.Application.Contracts;
using ProductSales.Application.Services;
using ProductSales.Domain.Entities;
using Xunit;

namespace ProductSales.UnitTests;

public sealed class ProductDashboardServiceTests
{
    [Fact]
    public async Task Aggregates_sales_per_product()
    {
        var logger = new Mock<ILogger<ProductDashboardService>>();

        var service = new ProductDashboardService(new Stub(), logger.Object);

        var dashboard = await service.GetAsync(default);

        dashboard.TotalProducts.Should().Be(1);
        dashboard.TotalQuantitySold.Should().Be(5);
        dashboard.TotalRevenue.Should().Be(72m);
        dashboard.SalesSummary.Single().TransactionCount.Should().Be(2);
    }

    private class Stub : IProductRepository
    {
        public Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyList<Product>>
            (
                [
                    new()
                    {
                        Id = 1,
                        Description = "Apples",
                        SalePrice = 20.00m,
                        Category = "Fruit",
                        Image = "x"
                    }
                ]
            );
        }

        public Task<IReadOnlyList<ProductSale>> GetProductSalesAsync(int productId, CancellationToken cancellationToken)
        {
            return Task.FromResult<IReadOnlyList<ProductSale>>
            (
                [
                    new()
                    {
                        SaleId = 1,
                        ProductId = 1,
                        SalePrice = 16m,
                        SaleQty = 3
                    },
                    new()
                    {
                        SaleId = 2,
                        ProductId = 1,
                        SalePrice = 12m,
                        SaleQty = 2
                    }
                ]
            );
        }
    }
}