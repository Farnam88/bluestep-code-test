using api.Domain.Shared.DataWrapper;

namespace api.Domain.Shared.CustomExceptions.Shared;
/// <summary>
/// The base class for custom exceptions
/// </summary>
public abstract class BaseException : Exception
{
    /// <summary>
    /// The error code that each custom exception has and it should be unique
    /// </summary>
    protected abstract long ErrorCode { get; }

    /// <summary>
    /// The message to be displayed(Non technical and human readable) 
    /// </summary>
    public string DisplayMessage { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="displayMessage"  cref="string">The message to be displayed(Non technical and human readable).</param>
    /// <param name="exceptionMessage" cref="string">The technical error message.</param>
    /// <param name="errorDetails" cref="ErrorDetail">Additional data about the error that occurred.</param>
    /// <param name="metaData" cref="DataWrapper.MetaData">Additional data to pass.</param>
    /// <param name="innerException" cref="Exception">Possible inner exception to append to the exception.</param>
    protected BaseException(string displayMessage = ExceptionDisplayErrorMessages.UnhandledException,
        string exceptionMessage = ExceptionErrorMessages.UnhandledException,
        IReadOnlyList<ErrorDetail> errorDetails = null!,
        IReadOnlyList<MetaData> metaData = null!,
        Exception innerException = null!) : base(exceptionMessage, innerException)
    {
        DisplayMessage = displayMessage;
        ErrorDetails = errorDetails ?? new List<ErrorDetail>();
        MetaData = metaData ?? new List<MetaData>();
    }

    /// <summary>
    /// Error code
    /// </summary>
    public long Code => ErrorCode;

    /// <summary>
    /// Additional data about the error that occurred.
    /// </summary>
    public IReadOnlyList<ErrorDetail> ErrorDetails { get; }
    
    /// <summary>
    /// Additional data to pass.
    /// </summary>
    public IReadOnlyList<MetaData> MetaData { get; }
}