using EGeek.Products.UseCase;
using Microsoft.AspNetCore.Builder;

namespace EGeek.Products;

public static class ProductConfigApp
{
    public static void Apply(WebApplication app)
    {
        app.MapPost("v1/products", CreateProductUseCase.Action);
    }
}