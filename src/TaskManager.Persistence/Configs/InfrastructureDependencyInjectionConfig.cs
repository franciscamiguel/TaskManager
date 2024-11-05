using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Contracts.Repositories;
using TaskManager.Persistence.Repositories;

namespace TaskManager.Persistence.Configs;

public static class InfrastructureDependencyInjectionConfig
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration);

        services.AddScoped<ITaskRepository, TaskRepository>();

        return services;
    }
}