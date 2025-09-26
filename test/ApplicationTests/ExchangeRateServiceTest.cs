using System.Threading;
using System.Threading.Tasks;
using api.Application.Services;
using api.Domain.Shared.CustomExceptions;
using api.Domain.Shared.DataWrapper;
using api.ExternalServices.Services;
using api.ExternalServices.Services.Contracts;

namespace apitests.ApplicationTests;

public class ExchangeRateServiceTest
{
    private readonly IExchangeRateService _sut;
    private readonly Mock<IBlueStepExternalService> _blueStepExternalServiceMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _ct = CancellationToken.None;

    public ExchangeRateServiceTest()
    {
        _fixture = new Fixture();
        _blueStepExternalServiceMock = new();
        _sut = new ExchangeRateService(_blueStepExternalServiceMock.Object);
    }

    [Fact]
    public async Task GetExchangeRate_ShouldReturnAccount_OnSuccess()
    {
        //Arrange

        var exchangeRateResponseDto = _fixture.Create<ExchangeRateDto>();
        var expectedResultFromExternalService = ResultModel<ExchangeRateDto>.Success(exchangeRateResponseDto);

        _blueStepExternalServiceMock.Setup(s => s.GetExchangeRate(_ct))
            .ReturnsAsync(expectedResultFromExternalService)
            .Verifiable();

        //Act`
        var actualResult = await _sut.GetExchangeRate(_ct);

        //Assert

        _blueStepExternalServiceMock.Verify(s => s.GetExchangeRate(_ct), Times.Once);

        actualResult.Currencies.Count.ShouldBeEquivalentTo(
            expectedResultFromExternalService.Result.Currencies.Count);
        actualResult.Name.ShouldBeEquivalentTo(expectedResultFromExternalService.Result.Name);
    }

    [Fact]
    public async Task GetExchangeRate_ShouldThrowExternalServiceException_OnFail()
    {
        //Arrange

        var expectedResultFromExternalService = ResultModel<ExchangeRateDto>.Fail(new ExternalServiceException());

        _blueStepExternalServiceMock.Setup(s => s.GetExchangeRate(_ct))
            .ReturnsAsync(expectedResultFromExternalService)
            .Verifiable();

        //Act`and assert
        await Should.ThrowAsync<ExternalServiceException>(async () => await _sut.GetExchangeRate(_ct));

        //Assert

        _blueStepExternalServiceMock.Verify(s => s.GetExchangeRate(_ct), Times.Once);
    }
}