using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductMgmt.Application.Services;
using ProductMgmt.Domain.Interfaces;
using ProductMgmt.Persistence.Data;
using ProductMgmt.Persistence.Repositories;
using ProductMgmt.Persistence.Services;

namespace ProductMgmt.Persistence.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("productConnectionString")));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}