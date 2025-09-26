using api.Domain.Shared.CustomExceptions.Shared;
using api.Domain.Shared.DataWrapper;

namespace api.Domain.Shared.CustomExceptions;

public sealed class ExternalServiceException(
    string displayMessage = ExceptionDisplayErrorMessages.ExternalServiceException,
    string exceptionMessage = ExceptionErrorMessages.ExternalServiceException,
    IReadOnlyList<ErrorDetail> errorDetails = null!,
    IReadOnlyList<MetaData> metaData = null!,
    Exception innerException = null!)
    : BaseException(displayMessage, exceptionMessage, errorDetails, metaData, innerException)
{
    protected override long ErrorCode => ExceptionErrorCodes.ExternalServiceException;
}