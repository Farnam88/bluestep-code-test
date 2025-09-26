using api.Domain.Shared.Caching;
using api.Domain.Shared.Configurations;

namespace api.Domain.DependencyInjections;

public static class DomainDependencyInjection
{
    public static void RegisterDomainServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ExternalServicesConfig>(builder.Configuration.GetSection(ExternalServicesConfig.SectionName));
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
    }
}