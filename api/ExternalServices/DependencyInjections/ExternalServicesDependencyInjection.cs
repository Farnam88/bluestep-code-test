using api.Domain.Shared.Configurations;
using api.ExternalServices.Services;

namespace api.ExternalServices.DependencyInjections;

public static class ExternalServicesDependencyInjection
{
    public static void RegisterExternalServices(this WebApplicationBuilder builder)
    {
        var externalServiceConfig = builder.Configuration.GetSection(ExternalServicesConfig.SectionName)
            .Get<ExternalServicesConfig>() ?? throw new("ExternalServices config not found");


        builder.Services.AddHttpClient<IBlueStepExternalService,BlueStepExternalService>(externalServiceConfig.BlueStep.Name,
                client => { client.BaseAddress = new Uri(externalServiceConfig.BlueStep.BaseUrl); })
            //Added the standard version for simplicity
            .AddStandardResilienceHandler();
    }
}