using api.Domain.Models;
using api.Domain.Shared.Configurations;
using api.ExternalServices.Services;
using Microsoft.Extensions.Options;

namespace api.Application.Services;

public interface IAccountService
{
    Task<Account> GetAccount(CancellationToken ct = default);
}

public class AccountService : IAccountService
{
    private readonly IBlueStepExternalService _externalService;

    public AccountService(IBlueStepExternalService externalService)
    {
        _externalService = externalService;
    }

    public async Task<Account> GetAccount(CancellationToken ct = default)
    {
        var resultModel = await _externalService.GetAccount(ct);
        if (resultModel.IsSuccess && resultModel.Result != null)
        {
            return new Account
            {
                AccountNumber = resultModel.Result.AccountNumber,
                Balance = resultModel.Result.Balance,
                Currency = resultModel.Result.Currency,
                Transactions = resultModel.Result.Transactions.Select(s => new Transaction
                {
                    Balance = s.Balance,
                    Date = s.Date
                }).ToList()
            };
        }

        throw resultModel.GetException();
    }
}