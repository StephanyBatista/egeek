using System.Reflection;
using EGeek.Products.Domain;
using EGeek.Products.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EGeek.Products;

public static class ProductsModularExtension
{
    public static void Apply(
        IServiceCollection services, 
        ConfigurationManager configuration,
        List<Assembly> mediatRAssemblies)
    {
        services.AddDbContext<ProductDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("ProductConnection"), 
                config => config.MigrationsHistoryTable("__EFMigrationsHistory", "product"));
        });
        
        services.AddScoped<IProductRepository, ProductRepository>();
        
        mediatRAssemblies.Add(typeof(ProductsModularExtension).Assembly);
    }
}