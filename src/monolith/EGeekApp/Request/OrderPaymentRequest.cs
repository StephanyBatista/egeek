namespace EGeekApp.Request;

public class OrderPaymentRequest
{
    public string Address { get; set; }
    public string CreditCardNumber { get; set; }
    public string CreditCardCode { get; set; }
}