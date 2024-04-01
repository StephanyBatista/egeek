using EGeekapp.Users;

namespace EGeekdomain;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public User CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public User? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
