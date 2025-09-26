using System.Text.Json.Serialization;

namespace api.Domain.Shared.DataWrapper;

/// <summary>
///  Additional data about the error that occurred.
/// </summary>
public class ErrorDetail
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="title">The title of the object</param>
    /// <param name="value">The value of the object. It should not contain any sensitive data.</param>
    /// <param name="isExpose">Expose the error detail to the outside world(Client side)</param>
    public ErrorDetail(string title, string value, bool isExpose = false)
    {
        Title = title;
        Value = value;
        Expose = isExpose;
    }

    /// <summary>
    /// Title of the error to be used either as a key or to showcase.
    /// </summary>
    [JsonInclude]
    public string Title { get; private set; }

    /// <summary>
    /// The actual value of the error.
    /// It should not contain any sensitive data.
    /// </summary>
    [JsonInclude]
    public string Value { get; private set; }

    /// <summary>
    /// Indicates the exposure of the object to end user.
    /// Set to true if you want to expose it.
    /// </summary>
    [JsonIgnore]
    public bool Expose { get; }

    /// <summary>
    /// Overridden ToString
    /// </summary>
    /// <returns>Returns Title : Value</returns>
    public override string ToString()
    {
        return $"{Title} : {Value}";
    }
}