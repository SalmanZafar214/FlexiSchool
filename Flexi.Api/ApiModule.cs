using Flexi.Infrastructure;

namespace Flexi.Api;

public static class ApiModule
{
    public static void AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterInfrastructureDependencies(configuration);
    }
}