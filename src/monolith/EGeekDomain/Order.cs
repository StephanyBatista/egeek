namespace EGeekDomain;

public enum OrderStatus
{
    Pending,
    Paid,
    PaymentNotAuthorized
}

public enum ShippingStatus
{
    Preparing,
    Sent,
    Delivered
}

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string? Address { get; set; }
    public string? CreditCardMasked { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public User CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public User UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public OrderStatus Status { get; set; }
    public Guid TransactionId { get; set; }
    public DateTime TransactionDate { get; set; }
    public ShippingStatus ShippingStatus { get; set; }
    public DateTime ShippingDate { get; set; }
}