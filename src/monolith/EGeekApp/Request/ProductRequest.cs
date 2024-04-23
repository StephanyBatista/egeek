namespace EGeekApp.Request;

public class ProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public int WeightInGrams { get; set; }
    public int HeightInCentimeters { get; set; }
    public int WidthInCentimeters { get; set; }
}