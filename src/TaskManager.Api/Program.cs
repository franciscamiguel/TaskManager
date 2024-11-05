using TaskManager.Api.Configs;
using TaskManager.Api.Configs.App;
using TaskManager.Persistence.Configs;
using TaskManager.S3;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.ConfigureApi()
    .AddAppServices()
    .ConfigureInfrastructure(configuration)
    .AddStorageStartup(configuration);

var app = builder.Build();

app.UseApi();

app.UseAuthorization();

await app.RunAsync();