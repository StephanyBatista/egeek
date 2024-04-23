namespace EGeekApp.Request;

public class OrderItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string Version { get; set; }
}