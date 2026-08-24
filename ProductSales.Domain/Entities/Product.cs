namespace ProductSales.Domain.Entities;

public class Product
{
    public int Id { get; init; }

    public required string Description { get; init; }

    public decimal SalePrice { get; init; }

    public required string Category { get; init; }

    public required string Image { get; init; }
}