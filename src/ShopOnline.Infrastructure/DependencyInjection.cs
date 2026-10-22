using Microsoft.Extensions.DependencyInjection;
using ShopOnline.Application.Services;
using ShopOnline.Infrastructure.Services;

namespace ShopOnline.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
