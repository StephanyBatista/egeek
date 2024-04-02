namespace EGeekDomain;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Address { get; set; }
    public string CreditCardMasked { get; set; }
    public List<OrderItem> tems { get; set; }
    public decimal TotalAmount { get; set; }
    public User CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public User UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}