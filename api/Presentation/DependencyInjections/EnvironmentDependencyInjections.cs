namespace api.Presentation.DependencyInjections;

public abstract class EnvironmentDependencyInjections
{
    public static WebApplicationBuilder RegisterEnvironments(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddJsonFile("appsettings.json", false, true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName.ToLower()}.json", true,
            true);
        builder.Configuration.AddEnvironmentVariables();
        return builder;
    }
}