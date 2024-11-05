using FluentValidation;
using TaskManager.Domain.Contracts.Entities;
using TaskManager.Domain.Models.TaskAggregate.Services;
using TaskManager.Domain.Models.TaskAggregate.Validators;
using TaskManager.Domain.Notifier;
using TaskManager.Domain.Services;
using TaskManager.Domain.Validator;
using TaskManager.S3.Model.File;

namespace TaskManager.Api.Configs.App;

public static class RegisterAppServicesConfig
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IFileService, FileService>();
        services.AddSingleton<IUriService, UriService>();
        services.AddScoped<INotifierMessage, NotifierMessage>();
        services.AddScoped<IValidatorGeneric, ValidatorFactory>();

        services.AddValidatorsFromAssemblyContaining<RegisterTaskDtoValidador>();

        return services;
    }
}