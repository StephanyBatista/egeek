namespace EGeek.Products.UseCase;

public class CreateProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public int QuantityInStock { get; set; }
    public int WeightInGrams { get; set; }
    public int HeightInCentimeters { get; set; }
    public int WidthInCentimeters { get; set; }
}