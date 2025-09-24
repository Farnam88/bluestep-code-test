using api.Models;

namespace api.Services;

public interface ITransactionService
{
    (DateTime? start, DateTime? end, decimal highestBalanceChange) GetHighestPositiveBalanceChange(List<Transaction> transactions);
}

public class TransactionService:ITransactionService
{
    public (DateTime? start, DateTime? end, decimal highestBalanceChange) GetHighestPositiveBalanceChange(List<Transaction> transactions)
    {
        throw new NotImplementedException();
    }
}