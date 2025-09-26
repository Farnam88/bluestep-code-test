using System.Text.Json.Serialization;

namespace api.Domain.Shared.DataWrapper;

/// <summary>
/// Empty Success message, you can use it instead of object or string
/// </summary>
public struct Success
{
    /// <summary>
    /// Successful
    /// </summary>
    [JsonIgnore]
    public static Success Succeeded => SuccessOperationType.Succeeded;

    /// <summary>
    /// Created
    /// </summary>
    [JsonIgnore]
    public static Success Created => SuccessOperationType.Created;

    /// <summary>
    /// Updated
    /// </summary>
    [JsonIgnore]
    public static Success Updated => SuccessOperationType.Updated;

    /// <summary>
    /// Deleted
    /// </summary>
    [JsonIgnore]
    public static Success Deleted => SuccessOperationType.Deleted;

    /// <summary>
    /// Success Message
    /// </summary>
    [JsonIgnore]
    public string Message { get; }

    private static readonly string DefaultMessage = $"Operation {SuccessOperationType.Succeeded.ToString()}";

    /// <summary>
    /// Construct the success result based on the Operation type
    /// </summary>
    /// <param name="operation"></param>
    public Success(SuccessOperationType operation)
    {
        if (operation == SuccessOperationType.Succeeded)
            Message = DefaultMessage;
        Message = operation.ToString();
    }

    /// <summary>
    /// Construct success message with a string
    /// </summary>
    /// <param name="message"></param>
    /// <param name="code">Success code</param>
    public Success(string message = "")
    {
        if (!string.IsNullOrEmpty(message))
            Message = message;
        else
            Message = DefaultMessage;
    }


    /// <summary>
    /// Implicit operation from <see cref="SuccessOperationType"/> to Success object
    /// </summary>
    /// <param name="operation"><see cref="SuccessOperationType"/></param>
    /// <returns></returns>
    public static implicit operator Success(SuccessOperationType operation) =>
        new(operation);

    /// <summary>
    /// Implicit operation from <see cref="string"/> to success object
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static implicit operator Success(string message) => new(message);
}

/// <summary>
/// The type
/// </summary>
public enum SuccessOperationType
{
    /// <summary>
    /// General successful operation
    /// </summary>
    Succeeded = 1,

    /// <summary>
    /// Successful Create
    /// </summary>
    Created = 2,

    /// <summary>
    /// Successful Update
    /// </summary>
    Updated = 3,

    /// <summary>
    /// Successful Delete
    /// </summary>
    Deleted = 4
}