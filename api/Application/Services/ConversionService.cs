using api.Domain.Models;

namespace api.Application.Services;

public interface IConversionService
{
    Task<Account> GetConvertedAccount(string currency);
}

public class ConversionService : IConversionService
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IAccountService _accountService;

    public ConversionService(IExchangeRateService exchangeRateService, IAccountService accountService)
    {
        _exchangeRateService = exchangeRateService;
        _accountService = accountService;
    }

    public async Task<Account> GetConvertedAccount(string currency)
    {
        var account = await _accountService.GetAccount();

        if (currency.Equals(account.Currency, StringComparison.InvariantCultureIgnoreCase))
            return account;
        
        var exchanges = await _exchangeRateService.GetExchangeRate();
        if (!exchanges.Currencies.Any(a => a.Name.Equals(currency, StringComparison.InvariantCultureIgnoreCase)))
            throw new ArgumentException("Currency does not exist");
        
        var exchangeCurrency =
            exchanges.Currencies.First(f => f.Name.Equals(currency, StringComparison.InvariantCultureIgnoreCase));


        account.Currency = exchangeCurrency.Name;
        account.Balance *= exchangeCurrency.ExchangeRate;
        foreach (var transaction in account.Transactions)
        {
            transaction.Balance *= exchangeCurrency.ExchangeRate;
        }

        return account;
    }
}