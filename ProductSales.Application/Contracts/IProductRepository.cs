using ProductSales.Domain.Entities;

namespace ProductSales.Application.Contracts
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken ct);
        Task<IReadOnlyList<ProductSale>> GetProductSalesAsync(int productId, CancellationToken ct);
    }
}