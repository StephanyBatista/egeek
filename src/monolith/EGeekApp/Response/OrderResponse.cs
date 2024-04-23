namespace EGeekApp.Response;

public class OrderResponse
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public string Address { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemResponse> Items { get; set; }
    public string ShippingStatus { get; set; }
    public DateTime ShippingDate { get; set; }
}