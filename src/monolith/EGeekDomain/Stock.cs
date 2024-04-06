namespace EGeekDomain;

public class Stock
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Color { get; set; }
    public Product Product { get; set; }
}