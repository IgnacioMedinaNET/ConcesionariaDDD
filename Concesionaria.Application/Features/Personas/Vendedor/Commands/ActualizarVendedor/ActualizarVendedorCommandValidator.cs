namespace Application.Features.Personas.Commands.ActualizarVendedor;

using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Common.Enums;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class ActualizarVendedorCommandValidator
    : AbstractValidator<ActualizarVendedorCommand>
{
    public ActualizarVendedorCommandValidator(
        IVendedorRepository repository,
        ICurrentUserService currentUser
    )
   {
        RuleFor(c => c.NombreCompleto).NotEmpty().WithMessage("El nombre es obligatorio.");

        RuleFor(c => c.Dni)
            .Length(8)
            .WithMessage("El DNI debe tener 8 dígitos")
            .CustomAsync(
                async (dni, context, ct) =>
                {
                    var empresaId = currentUser.EmpresaId;
                    var estado = await repository.ExistePorDniExcluyendoIdAsync(dni.Trim(), empresaId, context.InstanceToValidate.VendedorId);

                    if (estado == EstadoExistencia.Activo)
                        context.AddFailure("Ya existe un vendedor activo con ese DNI.");

                    if (estado == EstadoExistencia.Desactivado)
                        context.AddFailure(
                            "Ya se encuentra un vendedor inactivo con ese DNI. Puede reactivarlo."
                        );
                }
            );

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("El Email es obligatorio")
            .CustomAsync(
                async (email, context, ct) =>
                {
                    var empresaId = currentUser.EmpresaId;
                    var estado = await repository.ExistePorEmailExcluyendoIdAsync(email.Trim(), empresaId, context.InstanceToValidate.VendedorId);

                    if (estado == EstadoExistencia.Activo)
                        context.AddFailure("Ya existe un vendedor activo con ese Email.");

                    if (estado == EstadoExistencia.Desactivado)
                        context.AddFailure(
                            "Ya se encuentra un vendedor inactivo con ese Email. Puede reactivarlo."
                        );
                }
            );

        // RuleFor(c => c.ComisionPorcentaje)
        //     .GreaterThan(0)
        //     .WithMessage("La comisión debe ser mayor a 0.");

        RuleFor(c => c.SucursalId).NotEmpty().WithMessage("La sucursal es obligatoria");

        RuleFor(c => c.LocalidadId).NotEmpty().WithMessage("La localidad es obligatoria");
    }
}
