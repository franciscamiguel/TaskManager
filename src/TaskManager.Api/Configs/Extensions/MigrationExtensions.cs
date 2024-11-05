using Microsoft.EntityFrameworkCore;
using TaskManager.Persistence;

namespace TaskManager.Api.Configs.Extensions;

public static class MigrationExtensions
{
    /// <summary>
    ///     Aplica Migrations ao executar a aplicação
    /// </summary>
    /// <param name="app"></param>
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dataContext.Database.Migrate();
    }
}