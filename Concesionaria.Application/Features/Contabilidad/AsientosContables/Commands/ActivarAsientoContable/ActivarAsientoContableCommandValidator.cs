using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace Application.Features.Contabilidad.AsientosContables.Commands.ActivarAsientoContable;

public class ActivarAsientoContableCommandValidator
    : AbstractValidator<ActivarAsientoContableCommand>
{
    public ActivarAsientoContableCommandValidator(
        IAsientoContableRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(a => a.AsientoId)
            .NotEmpty()
            .WithMessage("El identificador del asiento es obligatorio.");
    }
}