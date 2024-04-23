namespace EGeekApp.Integration;

public class ResultPayment
{
    public bool Success { get; set; }
    public Guid TransactionId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string CreditCardMasked { get; set; }
}