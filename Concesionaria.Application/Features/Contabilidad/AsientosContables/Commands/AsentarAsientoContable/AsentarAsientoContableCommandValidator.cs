using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Contabilidad.AsientosContables.Enum;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace Application.Features.Contabilidad.AsientosContables.Commands.AsentarAsientoContable;

public class AsentarAsientoContableCommandValidator
    : AbstractValidator<AsentarAsientoContableCommand>
{
    public AsentarAsientoContableCommandValidator(
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

                if (asiento.Estado == EstadoAsiento.ASENTADO)
                {
                    context.AddFailure("El asiento ya se encuentra ASENTADO.");
                    return;
                }

                if (asiento.Estado == EstadoAsiento.ANULADO)
                {
                    context.AddFailure("No se puede asentar un asiento que se encuentra ANULADO.");
                    return;
                }

                if (asiento.Detalles == null || asiento.Detalles.Count < 2)
                {
                    context.AddFailure("El asiento debe contener al menos dos líneas de detalle para poder asentarse.");
                    return;
                }

                var totalDebe = asiento.Detalles.Sum(d => d.Debe);
                var totalHaber = asiento.Detalles.Sum(d => d.Haber);

                if (totalDebe != totalHaber)
                {
                    context.AddFailure($"El asiento no está balanceado. Total Debe: {totalDebe}, Total Haber: {totalHaber}.");
                }
            });
    }
}