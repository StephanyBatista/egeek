using EGeek.Products.Domain;

namespace EGeek.Products.Infra;

internal class ProductRepository(ProductDbContext context) : IProductRepository
{
    public async Task<int> Create(Product product)
    {
        var entity = await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return entity.Entity.Id;
    }
}