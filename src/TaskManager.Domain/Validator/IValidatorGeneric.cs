using FluentValidation;

namespace TaskManager.Domain.Validator;

public interface IValidatorGeneric
{
    IValidator<T> GetValidator<T>();
}