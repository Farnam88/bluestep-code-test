using api.Domain.Shared.Caching;
using api.Domain.Shared.CustomExceptions;
using api.Domain.Shared.DataWrapper;
using api.ExternalServices.Services.Contracts;

namespace api.ExternalServices.Services;

public interface IBlueStepExternalService
{
    Task<ResultModel<AccountDto?>> GetAccount(CancellationToken ct = default);
    Task<ResultModel<ExchangeRateDto?>> GetExchangeRate(CancellationToken ct = default);
}

public class BlueStepExternalService : IBlueStepExternalService
{
    private readonly HttpClient _httpClient;
    private const string AccountsEndpoint = "account.json";
    private const string AccountsCacheKey = "account-detail";
    private const string ExchangeRateEndpoint = "currencies.json";
    private const string ExchangeRateCacheKey = "exchange-rate";
    private readonly IMemoryCacheService _cacheService;

    public BlueStepExternalService(HttpClient httpclient, IMemoryCacheService cacheService)
    {
        _cacheService = cacheService;
        _httpClient = httpclient;
    }

    public async Task<ResultModel<AccountDto?>> GetAccount(CancellationToken ct = default)
    {
        try
        {
            if (_cacheService.TryGet<AccountDto>(AccountsCacheKey, out var account))
            {
                return account;
            }

            var httpResponseMessage = await _httpClient.GetAsync(AccountsEndpoint, ct);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = await httpResponseMessage.Content.ReadFromJsonAsync<AccountDto>(ct);
                if (result != null)
                    _cacheService.Add(AccountsCacheKey, result, TimeSpan.FromSeconds(15));
                return result;
            }

            return new ExternalServiceException(errorDetails: new List<ErrorDetail>()
            {
                new("status-code", $"{httpResponseMessage.StatusCode.ToString()}"),
                new("message", $"{httpResponseMessage.ReasonPhrase}")
            });
        }
        catch (Exception ex)
        {
            return new ExternalServiceException(exceptionMessage: ex.Message, errorDetails: new List<ErrorDetail>()
            {
                new("error-message", $"{ex.Message}")
            }, innerException: ex);
        }
    }

    public async Task<ResultModel<ExchangeRateDto?>> GetExchangeRate(CancellationToken ct = default)
    {
        try
        {
            if (_cacheService.TryGet<ExchangeRateDto>(ExchangeRateCacheKey, out var exchangeRate))
            {
                return exchangeRate;
            }

            var httpResponseMessage = await _httpClient.GetAsync(ExchangeRateEndpoint, ct);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = await httpResponseMessage.Content.ReadFromJsonAsync<ExchangeRateDto>(ct);
                if (result != null)
                    _cacheService.Add(ExchangeRateCacheKey, result, TimeSpan.FromSeconds(15));
                return result;
            }

            return new ExternalServiceException(errorDetails: new List<ErrorDetail>()
            {
                new("status-code", $"{httpResponseMessage.StatusCode.ToString()}"),
                new("message", $"{httpResponseMessage.ReasonPhrase}")
            });
        }
        catch (Exception ex)
        {
            return new ExternalServiceException(exceptionMessage: ex.Message, errorDetails: new List<ErrorDetail>()
            {
                new("error-message", $"{ex.Message}")
            }, innerException: ex);
        }
    }
}