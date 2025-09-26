namespace api.Domain.Models;

public class ExchangeRate
{
    public required string Name { get; set; }
    public required List<Currency> Currencies { get; set; }
}