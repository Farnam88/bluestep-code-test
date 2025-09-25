using api.Models;

namespace api.Services;

public interface IConversionService
{
    Task<Account> GetConvertedAccount(string currency);
}

public class ConversionService:IConversionService
{
    public Task<Account> GetConvertedAccount(string currency)
    {
        if(currency == "NOT_VALID")
            throw new ArgumentException();
        return Task.FromResult(new Account
        {
            Balance = 110,
            Currency = currency,
            Transactions = new List<Transaction>()
            {
                new Transaction() { Date = DateTime.UtcNow.AddDays(-1).Date, Balance = 1 },
                new Transaction() { Date = DateTime.UtcNow.Date, Balance = 2 },
            }
        });
    }
}