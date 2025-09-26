using System.Threading;
using System.Threading.Tasks;
using api.Application.Services;
using api.Domain.Shared.CustomExceptions;
using api.Domain.Shared.DataWrapper;
using api.ExternalServices.Services;
using api.ExternalServices.Services.Contracts;

namespace apitests.ApplicationTests;

public class AccountServiceTest
{
    private readonly IAccountService _sut;
    private readonly Mock<IBlueStepExternalService> _blueStepExternalServiceMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _ct = CancellationToken.None;

    public AccountServiceTest()
    {
        _fixture = new Fixture();
        _blueStepExternalServiceMock = new();
        _sut = new AccountService(_blueStepExternalServiceMock.Object);
    }

    [Fact]
    public async Task GetAccount_ShouldReturnAccount_OnSuccess()
    {
        //Arrange

        var accountResponseDto = _fixture.Create<AccountDto>();
        var expectedResultFromExternalService = ResultModel<AccountDto>.Success(accountResponseDto);

        _blueStepExternalServiceMock.Setup(s => s.GetAccount(_ct))
            .ReturnsAsync(expectedResultFromExternalService)
            .Verifiable();

        //Act`
        var actualResult = await _sut.GetAccount(_ct);

        //Assert

        _blueStepExternalServiceMock.Verify(s => s.GetAccount(_ct), Times.Once);
        
        actualResult.Transactions.Count.ShouldBeEquivalentTo(
            expectedResultFromExternalService.Result.Transactions.Count);
        actualResult.Balance.ShouldBeEquivalentTo(expectedResultFromExternalService.Result.Balance);
        actualResult.Currency.ShouldBeEquivalentTo(expectedResultFromExternalService.Result.Currency);
        actualResult.AccountNumber.ShouldBeEquivalentTo(expectedResultFromExternalService.Result.AccountNumber);
    }
    
    [Fact]
    public async Task GetAccount_ShouldThrowExternalServiceException_OnFail()
    {
        //Arrange

        var expectedResultFromExternalService = ResultModel<AccountDto>.Fail(new ExternalServiceException());

        _blueStepExternalServiceMock.Setup(s => s.GetAccount(_ct))
            .ReturnsAsync(expectedResultFromExternalService)
            .Verifiable();

        //Act`and assert
        await Should.ThrowAsync<ExternalServiceException>(async () => await _sut.GetAccount(_ct));  

        //Assert

        _blueStepExternalServiceMock.Verify(s => s.GetAccount(_ct), Times.Once);
    }
}