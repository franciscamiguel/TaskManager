using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Domain.Validator;

public class ValidatorFactory(IServiceProvider serviceProvider) : IValidatorGeneric
{
    public IValidator<T> GetValidator<T>()
    {
        return serviceProvider.GetService<IValidator<T>>();
    }
}