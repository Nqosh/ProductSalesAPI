using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProductSales.Application.Contracts;

namespace ProductSales.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMemoryCache();

        services
            .AddOptions<ExternalApiOptions>()
            .Bind(configuration.GetSection(ExternalApiOptions.Section))
            .Validate(
                x => Uri.TryCreate(x.BaseUrl, UriKind.Absolute, out _),
                "A valid live API BaseUrl is required")
            .ValidateOnStart();

        services.AddHttpClient<IProductRepository, LiveProductRepository>(
            (serviceProvider, httpClient) =>
            {
                var options = serviceProvider
                    .GetRequiredService<IOptions<ExternalApiOptions>>()
                    .Value;

                httpClient.BaseAddress = new Uri(options.BaseUrl);
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                httpClient.DefaultRequestHeaders.Accept
                    .ParseAdd("application/json");

                httpClient.DefaultRequestHeaders.UserAgent
                    .ParseAdd("ProductSalesEnterprise/1.0");
            });

        return services;
    }
}