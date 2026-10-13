using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace Application.Features.Contabilidad.AsientosContables.Commands.AgregarAsientoContable;

public class AgregarAsientoContableCommandValidator
    : AbstractValidator<AgregarAsientoContableCommand>
{
    public AgregarAsientoContableCommandValidator(
        ICuentaRepository cuentaRepository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(a => a.Fecha)
            .NotEmpty()
            .WithMessage("La fecha del asiento es obligatoria.");

        RuleFor(a => a.Concepto)
            .NotEmpty()
            .WithMessage("El concepto del asiento es obligatorio.")
            .MaximumLength(250)
            .WithMessage("El concepto no puede superar los 250 caracteres.");

        RuleFor(a => a.Detalles)
            .CustomAsync(async (detalles, context, cancellationToken) =>
            {
                if (detalles == null || detalles.Count == 0)
                    return;

                var totalDebe = detalles.Sum(d => d.Debe);
                var totalHaber = detalles.Sum(d => d.Haber);

                if (totalDebe != totalHaber)
                {
                    context.AddFailure($"El asiento no está balanceado. Total Debe: {totalDebe}, Total Haber: {totalHaber}.");
                }

                foreach (var item in detalles)
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
            });
    }
}