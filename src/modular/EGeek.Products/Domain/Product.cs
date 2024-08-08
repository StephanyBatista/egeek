using EGeek.Products.UseCase;

namespace EGeek.Products.Domain;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int QuantityInStock { get; private set; }
    public int WeightInGrams { get; private set; }
    public int HeightInCentimeters { get; private set; }
    public int WidthInCentimeters { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    internal Product() {}

    public Product(CreateProductRequest request, string createdBy)
    {
        BadException.ThrowIfNullOrEmpt(request.Name, nameof(Name));
        BadException.ThrowIfNullOrEmpt(request.Description, nameof(Description));
        BadException.ThrowIf(request.Price <= 0, nameof(Price));
        BadException.ThrowIf(request.QuantityInStock <= 0, nameof(QuantityInStock));
        BadException.ThrowIf(request.WeightInGrams <= 0, nameof(WeightInGrams));
        BadException.ThrowIf(request.HeightInCentimeters <= 0, nameof(HeightInCentimeters));
        BadException.ThrowIf(request.WidthInCentimeters <= 0, nameof(WidthInCentimeters));
        Name = request.Name;
        Description = request.Description;
        Price = request.Price;
        QuantityInStock = request.QuantityInStock;
        WeightInGrams = request.WeightInGrams;
        HeightInCentimeters = request.HeightInCentimeters;
        WidthInCentimeters = request.WidthInCentimeters;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }
}