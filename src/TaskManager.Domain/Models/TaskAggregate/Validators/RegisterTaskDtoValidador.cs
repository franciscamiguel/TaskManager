using FluentValidation;
using TaskManager.Domain.Models.TaskAggregate.Dtos;

namespace TaskManager.Domain.Models.TaskAggregate.Validators;

public class RegisterTaskDtoValidador : AbstractValidator<RegisterTaskDto>
{
    public RegisterTaskDtoValidador()
    {
        RuleFor(t => t.Title)
            .NotEmpty()
            .NotNull()
            .WithMessage("Título não pode ser nulo")
            .MinimumLength(3)
            .WithMessage("Título deve ter pelo menos 3 caracteres");

        RuleFor(t => t.Description)
            .NotEmpty()
            .NotNull()
            .WithMessage("Descrição não pode ser nulo")
            .MinimumLength(3)
            .WithMessage("Descrição deve ter pelo menos 3 caracteres");
    }
}