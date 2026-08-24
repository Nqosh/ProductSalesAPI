using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductSales.Application.Services;

namespace ProductSales.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<
            IProductDashboardService,
            ProductDashboardService>();
        return services;
    }
}