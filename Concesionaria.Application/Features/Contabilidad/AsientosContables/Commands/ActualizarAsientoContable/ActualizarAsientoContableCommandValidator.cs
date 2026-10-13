using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Contabilidad.AsientosContables.Enum;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace Application.Features.Contabilidad.AsientosContables.Commands.ActualizarAsientoContable;

public class ActualizarAsientoContableCommandValidator
    : AbstractValidator<ActualizarAsientoContableCommand>
{
    public ActualizarAsientoContableCommandValidator(
        IAsientoContableRepository asientoRepository,
        ICuentaRepository cuentaRepository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(a => a.AsientoId)
            .NotEmpty()
            .WithMessage("El identificador del asiento es obligatorio.");

        RuleFor(a => a.Fecha)
            .NotEmpty()
            .WithMessage("La fecha del asiento es obligatoria.");

        RuleFor(a => a.Concepto)
            .NotEmpty()
            .WithMessage("El concepto del asiento es obligatorio.")
            .MaximumLength(250)
            .WithMessage("El concepto no puede superar los 250 caracteres.");

        RuleFor(a => a)
            .CustomAsync(async (command, context, cancellationToken) =>
            {
                var asiento = await asientoRepository.ObtenerPorIdConDetalleAsync(command.AsientoId, currentUser.EmpresaId);

                if (asiento == null)
                {
                    context.AddFailure("El asiento contable no existe.");
                    return;
                }

                if (asiento.Estado != EstadoAsiento.BORRADOR)
                {
                    context.AddFailure("Solo se pueden modificar asientos que se encuentren en estado BORRADOR.");
                    return;
                }

                if (command.Detalles != null && command.Detalles.Count > 0)
                {
                    var totalDebe = command.Detalles.Sum(d => d.Debe);
                    var totalHaber = command.Detalles.Sum(d => d.Haber);

                    if (totalDebe != totalHaber)
                    {
                        context.AddFailure($"El asiento no está balanceado. Total Debe: {totalDebe}, Total Haber: {totalHaber}.");
                    }

                    foreach (var item in command.Detalles)
                    {
                        if (item.Debe < 0 || item.Haber < 0)
                        {
                            context.AddFailure("Los importes de Debe y Haber no pueden ser negativos.");
                        }

                        if (item.Debe == 0 && item.Haber == 0)
                        {
                            context.AddFailure("Cada línea debe contener un importe en Debe o en Haber.");
                        }

                        var cuentaExiste = await cuentaRepository.ObtenerPorIdAsync(item.CuentaId, currentUser.EmpresaId);
                        if (cuentaExiste == null)
                        {
                            context.AddFailure($"La cuenta contable especificada ({item.CuentaId}) no existe o está inactiva.");
                        }
                    }
                }
            });
    }
}