using System;
using System.Collections.Generic;
using api.Models;
using api.Services;
using Xunit;

namespace apitests;

public class TransactionServiceTests
{
    private ITransactionService _sut;

    public TransactionServiceTests()
    {
        _sut = new TransactionService();
    }

    [Fact]
    public void Given_Two_Ascending_Values_Should_Return_Correct()
    {
        var transactions = new List<Transaction>()
        {
            new Transaction() { Date = DateTime.UtcNow.AddDays(-1).Date, Balance = 1 },
            new Transaction() { Date = DateTime.UtcNow.Date, Balance = 2 },
        };

        var (start, end, positiveBalanceChange) = _sut.GetHighestPositiveBalanceChange(transactions);

        Assert.Equivalent(DateTime.UtcNow.AddDays(-1).Date, start);
        Assert.Equivalent(DateTime.UtcNow.Date, end);
        Assert.Equivalent(1, positiveBalanceChange);
    }

    [Fact]
    public void Given_Three_Values_With_First_Pair_Being_Ascending_Should_Return_Correct()
    {
        var transactions = new List<Transaction>()
        {
            new Transaction() { Date = DateTime.UtcNow.AddDays(-2).Date, Balance = 1 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-1).Date, Balance = 2 },
            new Transaction() { Date = DateTime.UtcNow.Date, Balance = 1 },
        };

        var (start, end, positiveBalanceChange) = _sut.GetHighestPositiveBalanceChange(transactions);

        Assert.Equivalent(DateTime.UtcNow.AddDays(-2).Date, start);
        Assert.Equivalent(DateTime.UtcNow.AddDays(-1).Date, end);
        Assert.Equivalent(1, positiveBalanceChange);
    }

    [Fact]
    public void Given_Four_Values_With_Second_And_Third_Being_Ascending_Values_Should_Return_Correct()
    {
        var transactions = new List<Transaction>()
        {
            new Transaction() { Date = DateTime.UtcNow.AddDays(-3).Date, Balance = 2 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-2).Date, Balance = 1 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-1).Date, Balance = 2 },
            new Transaction() { Date = DateTime.UtcNow.Date, Balance = 1 },
        };

        var (start, end, positiveBalanceChange) = _sut.GetHighestPositiveBalanceChange(transactions);

        Assert.Equivalent(DateTime.UtcNow.AddDays(-2).Date, start);
        Assert.Equivalent(DateTime.UtcNow.AddDays(-1).Date, end);
        Assert.Equivalent(1, positiveBalanceChange);
    }

    [Fact]
    public void Given_Four_Values_With_First_And_Last_Values_Greatest_Earning_Should_Return_Correct()
    {
        var transactions = new List<Transaction>()
        {
            new Transaction() { Date = DateTime.UtcNow.AddDays(-3).Date, Balance = 0 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-2).Date, Balance = 1 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-1).Date, Balance = 2 },
            new Transaction() { Date = DateTime.UtcNow.Date, Balance = 3 },
        };

        var (start, end, positiveBalanceChange) = _sut.GetHighestPositiveBalanceChange(transactions);

        Assert.Equivalent(DateTime.UtcNow.AddDays(-3).Date, start);
        Assert.Equivalent(DateTime.UtcNow.Date, end);
        Assert.Equivalent(3, positiveBalanceChange);
    }

    [Fact]
    public void Given_All_Equal_Should_Return_Null_Values()
    {
        var transactions = new List<Transaction>()
        {
            new Transaction() { Date = DateTime.UtcNow.AddDays(-3).Date, Balance = 0 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-2).Date, Balance = 0 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-1).Date, Balance = 0 },
            new Transaction() { Date = DateTime.UtcNow.Date, Balance = 0 },
        };

        var (start, end, positiveBalanceChange) = _sut.GetHighestPositiveBalanceChange(transactions);

        Assert.Equivalent(null, start);
        Assert.Equivalent(null, end);
        Assert.Equivalent(0, positiveBalanceChange);
    }

    [Fact]
    public void Given_Descending_Should_Return_Null_Values()
    {
        var transactions = new List<Transaction>()
        {
            new Transaction() { Date = DateTime.UtcNow.AddDays(-3).Date, Balance = 3 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-2).Date, Balance = 2 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-1).Date, Balance = 1 },
            new Transaction() { Date = DateTime.UtcNow.Date, Balance = 0 },
        };

        var (start, end, positiveBalanceChange) = _sut.GetHighestPositiveBalanceChange(transactions);

        Assert.Equivalent(null, start);
        Assert.Equivalent(null, end);
        Assert.Equivalent(0, positiveBalanceChange);
    }

    [Fact]
    public void Given_Two_Peaks_With_Last_Being_Greatest_Should_Return_The_Last_Values()
    {
        var transactions = new List<Transaction>()
        {
            new Transaction() { Date = DateTime.UtcNow.AddDays(-4).Date, Balance = 4 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-3).Date, Balance = 5 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-2).Date, Balance = 4 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-1).Date, Balance = 3 },
            new Transaction() { Date = DateTime.UtcNow.Date, Balance = 5 },
        };

        var (start, end, positiveBalanceChange) = _sut.GetHighestPositiveBalanceChange(transactions);

        Assert.Equivalent(DateTime.UtcNow.AddDays(-1).Date, start);
        Assert.Equivalent(DateTime.UtcNow.Date, end);
        Assert.Equivalent(2, positiveBalanceChange);
    }

    [Fact]
    public void Given_Highest_Earning_From_Negative_To_Positive()
    {
        var transactions = new List<Transaction>()
        {
            new Transaction() { Date = DateTime.UtcNow.AddDays(-4).Date, Balance = 1 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-3).Date, Balance = -3 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-2).Date, Balance = 5 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-1).Date, Balance = -5 },
            new Transaction() { Date = DateTime.UtcNow.Date, Balance = 1 },
        };

        var (start, end, positiveBalanceChange) = _sut.GetHighestPositiveBalanceChange(transactions);

        Assert.Equivalent(DateTime.UtcNow.AddDays(-3).Date, start);
        Assert.Equivalent(DateTime.UtcNow.AddDays(-2).Date, end);
        Assert.Equivalent(8, positiveBalanceChange);
    }

    [Fact]
    public void Given_Highest_Earning_From_Fluctuating_Data()
    {
        var transactions = new List<Transaction>()
        {
            new Transaction() { Date = DateTime.UtcNow.AddDays(-8).Date, Balance = -5000 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-7).Date, Balance = 2000 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-6).Date, Balance = 0 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-5).Date, Balance = 2000 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-4).Date, Balance = -4000 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-3).Date, Balance = 0 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-2).Date, Balance = 5000 },
            new Transaction() { Date = DateTime.UtcNow.AddDays(-1).Date, Balance = 1000 },
            new Transaction() { Date = DateTime.UtcNow.Date, Balance = 6000 },
        };

        var (start, end, positiveBalanceChange) = _sut.GetHighestPositiveBalanceChange(transactions);

        Assert.Equivalent(DateTime.UtcNow.AddDays(-4).Date, start);
        Assert.Equivalent(DateTime.UtcNow.AddDays(-2).Date, end);
        Assert.Equivalent(9000, positiveBalanceChange);
    }
}