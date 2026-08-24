namespace ProductSales.Application.DTOs;
public sealed record ProductDto(int Id,string Description,decimal SalePrice,string Category,string Image);
public sealed record ProductSalesSummaryDto(int ProductId,string Description,string Category,int TransactionCount,int QuantitySold,decimal Revenue,decimal AverageSalePrice);
public sealed record DashboardDto(IReadOnlyList<ProductDto> Products,IReadOnlyList<ProductSalesSummaryDto> SalesSummary,int TotalProducts,int TotalTransactions,int TotalQuantitySold,decimal TotalRevenue,string? TopSellingProduct);
