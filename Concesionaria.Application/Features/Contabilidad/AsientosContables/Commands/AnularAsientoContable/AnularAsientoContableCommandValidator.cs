using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Contabilidad.AsientosContables.Enum;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace Application.Features.Contabilidad.AsientosContables.Commands.AnularAsientoContable;

public class AnularAsientoContableCommandValidator
    : AbstractValidator<AnularAsientoContableCommand>
{
    public AnularAsientoContableCommandValidator(
        IAsientoContableRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(a => a.AsientoId)
            .NotEmpty()
            .WithMessage("El identificador del asiento es obligatorio.");

        RuleFor(a => a.AsientoId)
            .CustomAsync(async (asientoId, context, cancellationToken) =>
            {
                var asiento = await repository.ObtenerPorIdConDetalleAsync(asientoId, currentUser.EmpresaId);

                if (asiento == null)
                {
                    context.AddFailure("El asiento contable no fue encontrado.");
                    return;
                }

                if (asiento.Estado == EstadoAsiento.ANULADO)
                {
                    context.AddFailure("El asiento ya se encuentra ANULADO.");
                }
            });
    }
}