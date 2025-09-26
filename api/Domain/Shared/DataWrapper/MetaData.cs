using System.Text.Json.Serialization;

namespace api.Domain.Shared.DataWrapper;

/// <summary>
/// Additional data to pass along with response.
/// </summary>
public class MetaData
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="title">The title of the object.</param>
    /// <param name="value">The value of the object.</param>
    public MetaData(string title, string value)
    {
        Title = title;
        Value = value;
    }

    /// <summary>
    /// Title of the metadata to be used either as a key or to showcase.
    /// </summary>
    [JsonInclude]
    public string Title { get; private set; }

    /// <summary>
    /// The actual value of the metadata.
    /// It should not contains any sensitive data.
    /// </summary>
    [JsonInclude]
    public string Value { get; private set; }

    /// <summary>
    /// Overridden ToString
    /// </summary>
    /// <returns>Returns Title : Value</returns>
    public override string ToString()
    {
        return $"{Title} : {Value}";
    }
}