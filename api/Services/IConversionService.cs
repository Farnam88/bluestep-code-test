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
        throw new NotImplementedException();
    }
}