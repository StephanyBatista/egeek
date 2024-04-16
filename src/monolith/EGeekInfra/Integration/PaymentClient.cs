using EGeekApp.Integration;

namespace EGeekInfra.Integration;

public class PaymentClient
{
    public async Task<ResultPayment> Pay(string creditCard, string code)
    {
        return await Task.FromResult(new ResultPayment
        {
            Success = true,
            TransactionId = Guid.NewGuid(),
            TransactionDate = DateTime.UtcNow,
            CreditCardMasked = creditCard.PadLeft(4, '*')
        });
    }
}