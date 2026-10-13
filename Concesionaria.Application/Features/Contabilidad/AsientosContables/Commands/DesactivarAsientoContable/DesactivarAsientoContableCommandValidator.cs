using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Contabilidad.AsientosContables.Enum;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace Application.Features.Contabilidad.AsientosContables.Commands.DesactivarAsientoContable;

public class DesactivarAsientoContableCommandValidator
    : AbstractValidator<DesactivarAsientoContableCommand>
{
    public DesactivarAsientoContableCommandValidator(
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

                if (asiento != null && asiento.Estado == EstadoAsiento.ASENTADO)
                {
                    context.AddFailure("No se puede desactivar un asiento que ya se encuentra ASENTADO.");
                }
            });
    }
}