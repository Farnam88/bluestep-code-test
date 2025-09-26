using api.Application.Services;
using api.Presentation.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace api.Presentation;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IConversionService _conversionService;
    private readonly ITransactionService _transactionService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IConversionService conversionService, ITransactionService transactionService,
        ILogger<AccountController> logger)
    {
        _conversionService = conversionService;
        _transactionService = transactionService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(AccountResponse), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IResult> Get([FromQuery] string currency)
    {
        try
        {
            var convertedAccount = await _conversionService.GetConvertedAccount(currency);
            var (highestEarningStart, highestEarningEnd, balanceChange) =
                _transactionService.GetHighestPositiveBalanceChange(convertedAccount.Transactions);

            //TODO: Mapper
            return Results.Ok(new AccountResponse
            {
                AccountNumber = convertedAccount.AccountNumber,
                Balance = convertedAccount.Balance,
                Currency = convertedAccount.Currency,
                HighestBalanceChangeStart = highestEarningStart,
                HighestBalanceChangeEndDate = highestEarningEnd,
                Transactions = convertedAccount.Transactions,
                HighestBalanceChange = balanceChange
            });
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest($"{currency} not supported");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Results.StatusCode(500);
        }
    }
}