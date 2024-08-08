namespace EGeek.Products.Domain;

internal interface IProductRepository
{
    Task<int> Create(Product product);
}