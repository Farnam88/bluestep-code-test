using api.Domain.Models;

namespace api.ExternalServices.Services.Contracts;

public class ExchangeRateDto
{
    public required string Name { get; set; }
    public required List<Currency> Currencies { get; set; }
}

public class CurrencyDto
{
    public required string Name { get; set; }
    public decimal ExchangeRate { get; set; }
}