using api.Domain.Models;

namespace api.Presentation.Contracts;

public record AccountResponse
{
    public required string AccountNumber { get; init; }
    public decimal Balance { get; init; }
    public required string Currency { get; init; }
    public List<Transaction> Transactions { get; init; } = new List<Transaction>();
    public DateTime? HighestBalanceChangeStart { get; init; }
    public DateTime? HighestBalanceChangeEndDate { get; init; }
    public decimal HighestBalanceChange { get; init; }
}