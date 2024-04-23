namespace EGeekDomain;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public int WeightInGrams { get; set; }
    public int HeightInCentimeters { get; set; }
    public int WidthInCentimeters { get; set; }
    public User CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public User? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
