using api.Models;

namespace api.Services;

public interface IExchangeRateService
{
    Task<ExchangeRate> GetExchangeRate();
}

public  class ExchangeRateService:IExchangeRateService
{
    public Task<ExchangeRate> GetExchangeRate()
    {
        throw new NotImplementedException();
    }
}