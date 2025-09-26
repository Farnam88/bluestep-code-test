using api.Domain.Models;
using api.ExternalServices.Services;

namespace api.Application.Services;

public interface IExchangeRateService
{
    Task<ExchangeRate> GetExchangeRate(CancellationToken ct = default);
}

public class ExchangeRateService : IExchangeRateService
{
    private readonly IBlueStepExternalService _externalService;

    public ExchangeRateService(IBlueStepExternalService externalService)
    {
        _externalService = externalService;
    }

    public async Task<ExchangeRate> GetExchangeRate(CancellationToken ct = default)
    {
        var resultModel = await _externalService.GetExchangeRate(ct);
        if (resultModel.IsSuccess && resultModel.Result != null)
        {
            return new ExchangeRate
            {
                Name = resultModel.Result.Name,
                Currencies = resultModel.Result.Currencies.Select(s => new Currency
                {
                    Name = s.Name,
                    ExchangeRate = s.ExchangeRate
                }).ToList()
            };
        }

        throw resultModel.GetException();
    }
}