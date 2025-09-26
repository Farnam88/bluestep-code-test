using api.Domain.Models;

namespace api.Application.Services;

public interface ITransactionService
{
    (DateTime? start, DateTime? end, decimal highestBalanceChange) GetHighestPositiveBalanceChange(
        List<Transaction> transactions);
}

public class TransactionService : ITransactionService
{
    public (DateTime? start, DateTime? end, decimal highestBalanceChange) GetHighestPositiveBalanceChange(
        List<Transaction> transactions)
    {
        if (transactions.Count < 2)
            return (null, null, 0);

        var sortedList = transactions.OrderBy(x => x.Date).ToArray();

        int startIndex = -1, endIndex = -1;
        decimal highestAscendingBalanceChange = 0;

        int left = 0;
        int right = 1;

        while (right < sortedList.Length)
        {
            // Restart the window by moving the left point to the right point
            if (sortedList[right].Balance < sortedList[right - 1].Balance)
            {
                // Evaluate the window [left, right-1] if it had at least 2 items
                if (right - 1 > left)
                {
                    decimal diff = sortedList[right - 1].Balance - sortedList[left].Balance;
                    if (diff > highestAscendingBalanceChange)
                    {
                        highestAscendingBalanceChange = diff;
                        startIndex = left;
                        endIndex = right - 1;
                    }
                }

                left = right; // reset window start
            }
            else
            {
                // continue finding non-decreasing values
                decimal diff = sortedList[right].Balance - sortedList[left].Balance;
                if (diff > highestAscendingBalanceChange)
                {
                    highestAscendingBalanceChange = diff;
                    startIndex = left;
                    endIndex = right;
                }
            }

            right++;
        }

        // In case no positive ascending change found we return the default value
        if (highestAscendingBalanceChange <= 0 || startIndex < 0 || endIndex < 0)
            return (null, null, 0);

        return (sortedList[startIndex].Date, sortedList[endIndex].Date, highestAscendingBalanceChange);
    }
}