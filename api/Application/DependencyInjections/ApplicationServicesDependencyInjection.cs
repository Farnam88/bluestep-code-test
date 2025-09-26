using api.Application.Services;

namespace api.Application.DependencyInjections;

public static class ApplicationServicesDependencyInjection
{
    public static void RegisterApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IConversionService, ConversionService>();
        builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
    }
}