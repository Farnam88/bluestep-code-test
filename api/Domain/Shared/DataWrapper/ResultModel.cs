using System.Text.Json.Serialization;
using api.Domain.Shared.CustomExceptions;
using api.Domain.Shared.CustomExceptions.Shared;

namespace api.Domain.Shared.DataWrapper;

/// <summary>
/// General purpose result model to encapsulate the response of services or apis.
/// </summary>
/// <typeparam name="TResult">Target output Result</typeparam>
public sealed class ResultModel<TResult>
{

    //Success
    private ResultModel(TResult result, string message = "Success",
        long resultCode = 1,
        IReadOnlyList<MetaData> metaData = null!)
    {
        IsSuccess = true;
        ResultCode = resultCode;
        Message = message;
        ErrorDetails = new List<ErrorDetail>();
        MetaData = metaData ?? new List<MetaData>();
        Result = result;
        _exception = null!;
    }

    //Fail
    private ResultModel(BaseException exception)
    {
        var details = exception.ErrorDetails is null
            ? new List<ErrorDetail>()
            : exception.ErrorDetails
                .Where(w => w.Expose)
                .ToList();
        IsSuccess = false;
        ResultCode = exception.Code;
        Message = exception.DisplayMessage;
        ErrorDetails = details;
        MetaData = exception.MetaData ?? new List<MetaData>();
        Result = default!;
        _exception = exception;
    }

    /// <summary>
    /// The resulting code could be a success or failure.
    /// Generally, a positive value indicates a successful operation and negative ones result in failure.
    /// <example>Success: 1256 Fail: -1500</example>
    /// </summary>
    [JsonInclude]
    public long ResultCode { get; private set; }

    /// <summary>
    /// A message to pass along with the result to showcase.
    /// It should always be brief and not contain any sensitive data.
    /// </summary>
    [JsonInclude]
    public string Message { get; private set; }

    /// <summary>
    /// Indication of succeed or failed operation.
    /// Value 'true' is when the operation was successful and the 'False' value indicates a failure in the operation.
    /// </summary>
    [JsonInclude]
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// Additional data about the error that occurred.
    /// It should not contain any sensitive data.
    /// </summary>
    [JsonInclude]
    public IReadOnlyList<ErrorDetail> ErrorDetails { get; private set; }

    /// <summary>
    /// Additional data to pass on success operations.
    /// It should not contain any sensitive data.
    /// </summary>
    [JsonInclude]
    public IReadOnlyList<MetaData> MetaData { get; private set; }

    /// <summary>
    /// The Result object that is supposed to be sent.
    /// </summary>
    [JsonInclude]
    public TResult Result { get; private set; }

    private readonly BaseException _exception;

    /// <summary>
    /// Creates Success ResultModel
    /// </summary>
    /// <param name="result">The Result object that is supposed to be sent.</param>
    /// <param name="message">A message to pass along with the result to showcase.</param>
    /// <param name="resultCode">A positive value indicating success operation. default value = 1</param>
    /// <param name="metaData">Additional data to pass on success operations.</param>
    /// <returns>ResultModel object in success state.</returns>
    public static ResultModel<TResult> Success(TResult result, string message = "Success", long resultCode = 1,
        List<MetaData> metaData = null!)
    {
        return new ResultModel<TResult>(result, message, resultCode, metaData);
    }

    /// <summary>
    /// Creates failure ResultModel.
    /// </summary>
    /// <param name="exception">The custom exception object.</param>
    /// <returns>ResultModel object in fail state.</returns>
    public static ResultModel<TResult> Fail(BaseException exception)
    {
        return new ResultModel<TResult>(exception);
    }

    /// <summary>
    /// Getting the exception passed to the Fail function.
    /// Logging purpose.
    /// </summary>
    /// <returns></returns>
    public BaseException GetException()
    {
        return _exception;
    }
    
    /// <summary>
    /// Implicit successful result
    /// </summary>
    /// <param name="input">Result object to be returned</param>
    /// <exception cref="NullException">In case the result is null and the expected TResult is not</exception>
    /// <returns><see cref="ResultModel{TResult}"/></returns>
    public static implicit operator ResultModel<TResult>(TResult input)
    {
        return Success(input);
    }

    /// <summary>
    /// Implicit failed result
    /// </summary>
    /// <param name="exception"><see cref="BaseException"/></param>
    /// <returns><see cref="ResultModel{TResult}"/></returns>
    public static implicit operator ResultModel<TResult>(BaseException exception) => Fail(exception);
}