namespace api.ExternalServices.Services.Contracts;

public class AccountDto
{
    public required string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public required string Currency { get; set; }
    public List<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
}

public class TransactionDto
{
    public DateTime Date { get; set; }
    public decimal Balance { get; set; }
}