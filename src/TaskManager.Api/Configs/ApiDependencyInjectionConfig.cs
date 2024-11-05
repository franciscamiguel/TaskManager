using Asp.Versioning.ApiExplorer;
using TaskManager.Api.Configs.Extensions;
using TaskManager.Api.Configs.Swagger;

namespace TaskManager.Api.Configs;

public static class ApiDependencyInjectionConfig
{
    public static IServiceCollection ConfigureApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddVersionedSwagger();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        services.AddDistributedMemoryCache();

        return services;
    }

    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseCors(option =>
            option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseVersionedSwagger(provider);

        app.UseHttpsRedirection();
        app.MapControllers();

        // Use Local Extensions
        if (app.Environment.IsDevelopment())
            app.ApplyMigrations();

        if (app.Environment.IsEnvironment("Test"))
            app.MapControllers().AllowAnonymous();
        else
            app.MapControllers();

        return app;
    }
}