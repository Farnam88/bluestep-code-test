using api.Models;

namespace api.Services;

public interface IAccountService
{
    Task<Account> GetAccount();
}

public class AccountService : IAccountService
{
    public Task<Account> GetAccount()
    {
        throw new NotImplementedException();
    }
}