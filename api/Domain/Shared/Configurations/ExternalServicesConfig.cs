namespace api.Domain.Shared.Configurations;

public class ExternalServicesConfig
{
    public const string SectionName = "ExternalServicesConfig";
    public required ExternalServiceDetail BlueStep { get; set; }
}

public class ExternalServiceDetail

{
    public required string BaseUrl { get; set; }
    public required string Name { get; set; }
}