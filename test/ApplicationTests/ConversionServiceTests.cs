using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Application.Services;
using api.Domain.Models;

namespace apitests.ApplicationTests;

public class ConversionServiceTests
{
    private readonly IConversionService _sut;
    private readonly Mock<IExchangeRateService> _exchangeRateServiceMock;
    private readonly Mock<IAccountService> _accountServiceMock;
    private readonly CancellationToken _ct = It.IsAny<CancellationToken>();

    public ConversionServiceTests()
    {
        _exchangeRateServiceMock = new();
        _accountServiceMock = new();
        _sut = new ConversionService(_exchangeRateServiceMock.Object, _accountServiceMock.Object);
    }

    [Fact]
    public async Task
        Given_Account_And_ExchangeRate_When_Converting_A_Service_Then_It_Should_Convert_On_Found_Exchange_Rate()
    {
        //Assert

        var account = new Account
        {
            AccountNumber = "123456-1234",
            Balance = 100,
            Currency = "SEK",
            Transactions = new List<Transaction>
            {
                new Transaction() { Date = DateTime.UtcNow.AddDays(-2).Date, Balance = 10 },
                new Transaction() { Date = DateTime.UtcNow.AddDays(-1).Date, Balance = 20 },
            }
        };

        _accountServiceMock.Setup(s => s.GetAccount(_ct))
            .ReturnsAsync(account)
            .Verifiable();
        
        
        var exchangeRate = new ExchangeRate
        {
            Name = "SEK",
            Currencies = new List<Currency>()
            {
                new Currency()
                {
                    Name = "DKK",
                    ExchangeRate = 1.1m
                }
            }
        };

        _exchangeRateServiceMock.Setup(s => s.GetExchangeRate(_ct))
            .ReturnsAsync(exchangeRate)
            .Verifiable();
        
        //Act
        var actual = await _sut.GetConvertedAccount("DKK");

        //Assert
        actual.ShouldNotBeNull();
        actual.Currency.ShouldBeEquivalentTo("DKK");
        actual.Balance.ShouldBeEquivalentTo(110m);
        actual.Transactions.Count.ShouldBeEquivalentTo(account.Transactions.Count);
        _accountServiceMock.Verify(s => s.GetAccount(_ct), Times.Once);
        _exchangeRateServiceMock.Verify(s => s.GetExchangeRate(_ct), Times.Once);
    }

    [Fact]
    public async Task
        Given_Account_And_ExchangeRate_When_Converting_A_Service_With_Not_Found_Currency_It_Should_Throw()
    {
        var account = new Account
        {
            AccountNumber = "123456-1234",
            Balance = 100,
            Currency = "SEK"
        };

        _accountServiceMock.Setup(s => s.GetAccount(_ct))
            .ReturnsAsync(account)
            .Verifiable();

        var exchangeRate = new ExchangeRate
        {
            Name = "SEK",
            Currencies = new List<Currency>()
            {
                new Currency()
                {
                    Name = "DKK",
                    ExchangeRate = 1.1m
                }
            }
        };

        _exchangeRateServiceMock.Setup(s => s.GetExchangeRate(_ct))
            .ReturnsAsync(exchangeRate)
            .Verifiable();
        
        //Act and Assert
        await Should.ThrowAsync<ArgumentException>(async () => await _sut.GetConvertedAccount("NOT_VALID"));

        //Assert
        _exchangeRateServiceMock.Verify(s => s.GetExchangeRate(_ct), Times.Once);
        _accountServiceMock.Verify(s => s.GetAccount(_ct), Times.Once);
    }
    
    [Fact]
    public async Task
        GetConvertedAccount_ShouldNot_ThrowException_WhenAskedCurrency_ISEqualTo_AccountCurrency()
    {
        var account = new Account
        {
            AccountNumber = "123456-1234",
            Balance = 100,
            Currency = "SEK",
            Transactions = new List<Transaction>
            {
                new Transaction() { Date = DateTime.UtcNow.AddDays(-2).Date, Balance = 1 },
                new Transaction() { Date = DateTime.UtcNow.AddDays(-1).Date, Balance = 2 },
            }
        };

        _accountServiceMock.Setup(s => s.GetAccount(_ct))
            .ReturnsAsync(account)
            .Verifiable();
        
        //Act and Assert
        var result=await _sut.GetConvertedAccount("SEK");

        //Assert
        result.ShouldBeEquivalentTo(account);
        _exchangeRateServiceMock.Verify(s => s.GetExchangeRate(_ct), Times.Never);
        _accountServiceMock.Verify(s => s.GetAccount(_ct), Times.Once);
    }
}