using ProductSales.Application.DTOs;
namespace ProductSales.Application.Services;
public interface IProductDashboardService{Task<DashboardDto> GetAsync(CancellationToken ct);}
