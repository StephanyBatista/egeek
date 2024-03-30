using EGeekapp.Users;
using Microsoft.AspNetCore.Identity;

namespace EGeekdomain;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Address { get; set; }
    public string CreditCardMasked { get; set; }
    public List<Product> OrderItems { get; set; }
    public decimal TotalAmount { get; set; }
    public User CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public User UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}