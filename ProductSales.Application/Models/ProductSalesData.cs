using ProductSales.Domain.Entities;

namespace ProductSales.Application.Models;

public class ProductSalesData
{
    public required Product Product { get; init; }

    public required IReadOnlyList<ProductSale> Sales
    {
        get;
        init;
    }
}