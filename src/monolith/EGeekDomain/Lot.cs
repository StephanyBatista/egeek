namespace EGeekDomain;

public class Lot
{
    public int Id { get; set; }
    public int Number { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int Year { get; set; }
    public Product Product { get; set; }
}