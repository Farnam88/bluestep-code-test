using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using api.Domain.Shared.Caching;
using api.Domain.Shared.Configurations;
using api.ExternalServices.Services;
using api.ExternalServices.Services.Contracts;
using apitests.ExternalServiceTests.Stubs;
using Microsoft.Extensions.Options;
using Moq.Protected;

namespace apitests.ExternalServiceTests;

public class BlueStepExternalServiceTest
{
    // private readonly IBlueStepExternalService _sut;
    private readonly Mock<IMemoryCacheService> _memoryCacheServiceMock;
    private readonly CancellationToken _ct = It.IsAny<CancellationToken>();

    private readonly Fixture _fixture;

    public BlueStepExternalServiceTest()
    {
        _memoryCacheServiceMock = new();
        _fixture = new Fixture();
    }

    private StubHttpMessageHandler StubHandler(HttpResponseMessage response)
    {
        var handler = new StubHttpMessageHandler
        {
            Responder = (req, _) =>
            {
                return response;
            }
        };
        return handler;
    }

    [Fact]
    public async Task GetAccount_Uses_Cache_When_Present_OnSuccess()
    {
        //Arrange

        var handler = StubHandler(new HttpResponseMessage(HttpStatusCode.OK));

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.test"),
        };

        var cachedAccount = _fixture.Create<AccountDto>();
        _memoryCacheServiceMock.Setup(c => c.TryGet(It.IsAny<string>(), out cachedAccount))
            .Returns(true)
            .Verifiable();

        var sut = new BlueStepExternalService(httpClient, _memoryCacheServiceMock.Object);
        //Act
        var result = await sut.GetAccount(_ct);

        //Assert
        result.Result.ShouldBeEquivalentTo(cachedAccount);

        _memoryCacheServiceMock.Verify(c => c.Add(It.IsAny<string>(), It.IsAny<AccountDto>(),
                It.IsAny<TimeSpan>()),
            Times.Never());

        _memoryCacheServiceMock.Verify(c => c.TryGet(It.IsAny<string>(), out cachedAccount), Times.Once);
    }

    [Fact]
    public async Task GetAccount_FetchesDataFromExternalProvider_On_CacheMiss_And_Caches_Result_OnSuccess()
    {
        //Arrange

        // Arrange HTTP 200 with JSON body
        var externalProviderResponse = _fixture.Create<AccountDto>();
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(externalProviderResponse)
        };
        var handler = StubHandler(httpResponse);

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.test"),
        };

        AccountDto? ignored;
        _memoryCacheServiceMock.Setup(c => c.TryGet(It.IsAny<string>(), out ignored!))
            .Returns(false)
            .Verifiable();

        _memoryCacheServiceMock.Setup(s => s.Add(It.IsAny<string>(), It.IsAny<AccountDto>(), It.IsAny<TimeSpan>()))
            .Verifiable();

        var sut = new BlueStepExternalService(httpClient,
            _memoryCacheServiceMock.Object);
        //Act
        var actualResult = await sut.GetAccount(_ct);


        //Assert
        actualResult.ShouldNotBeNull();
        actualResult.Result.ShouldNotBeNull();
        actualResult.Result.AccountNumber.ShouldBeEquivalentTo(externalProviderResponse.AccountNumber);
        actualResult.Result.Balance.ShouldBeEquivalentTo(externalProviderResponse.Balance);
        actualResult.Result.Currency.ShouldBeEquivalentTo(externalProviderResponse.Currency);
        actualResult.Result.Transactions.Count.ShouldBeEquivalentTo(externalProviderResponse.Transactions.Count);

        // Cached
        _memoryCacheServiceMock.Verify(c => c.Add(It.IsAny<string>(), It.IsAny<AccountDto>(),
            It.IsAny<TimeSpan>()), Times.Once);
        _memoryCacheServiceMock.Verify(s => s.Add(It.IsAny<string>(), It.IsAny<AccountDto>(),
            It.IsAny<TimeSpan>()), Times.Once);
    }
    
        [Fact]
    public async Task GetExchangeRate_Uses_Cache_When_Present_OnSuccess()
    {
        //Arrange

        var handler = StubHandler(new HttpResponseMessage(HttpStatusCode.OK));

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.test"),
        };

        var cachedExchangeRate = _fixture.Create<ExchangeRateDto>();
        _memoryCacheServiceMock.Setup(c => c.TryGet(It.IsAny<string>(), out cachedExchangeRate))
            .Returns(true)
            .Verifiable();

        var sut = new BlueStepExternalService(httpClient, _memoryCacheServiceMock.Object);
        //Act
        var result = await sut.GetExchangeRate(_ct);

        //Assert
        result.Result.ShouldBeEquivalentTo(cachedExchangeRate);

        _memoryCacheServiceMock.Verify(c => c.Add(It.IsAny<string>(), It.IsAny<ExchangeRateDto>(),
                It.IsAny<TimeSpan>()),
            Times.Never());

        _memoryCacheServiceMock.Verify(c => c.TryGet(It.IsAny<string>(), out cachedExchangeRate), Times.Once);
    }

    [Fact]
    public async Task GetExchangeRate_FetchesDataFromExternalProvider_On_CacheMiss_And_Caches_Result_OnSuccess()
    {
        //Arrange

        // Arrange HTTP 200 with JSON body
        var externalProviderResponse = _fixture.Create<ExchangeRateDto>();
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(externalProviderResponse)
        };
        var handler = StubHandler(httpResponse);

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.test"),
        };

        ExchangeRateDto? ignored;
        _memoryCacheServiceMock.Setup(c => c.TryGet(It.IsAny<string>(), out ignored!))
            .Returns(false)
            .Verifiable();

        _memoryCacheServiceMock.Setup(s => s.Add(It.IsAny<string>(), It.IsAny<ExchangeRateDto>(), It.IsAny<TimeSpan>()))
            .Verifiable();

        var sut = new BlueStepExternalService(httpClient,
            _memoryCacheServiceMock.Object);
        //Act
        var actualResult = await sut.GetExchangeRate(_ct);


        //Assert
        actualResult.ShouldNotBeNull();
        actualResult.Result.ShouldNotBeNull();
        actualResult.Result.Name.ShouldBeEquivalentTo(externalProviderResponse.Name);
        actualResult.Result.Currencies.Count.ShouldBeEquivalentTo(externalProviderResponse.Currencies.Count);

        // Cached
        _memoryCacheServiceMock.Verify(c => c.Add(It.IsAny<string>(), It.IsAny<ExchangeRateDto>(),
            It.IsAny<TimeSpan>()), Times.Once);
        _memoryCacheServiceMock.Verify(s => s.Add(It.IsAny<string>(), It.IsAny<ExchangeRateDto>(),
            It.IsAny<TimeSpan>()), Times.Once);
    }
}