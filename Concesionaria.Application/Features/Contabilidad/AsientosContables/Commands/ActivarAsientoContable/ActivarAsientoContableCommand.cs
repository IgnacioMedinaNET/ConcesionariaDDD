using MediatR;

namespace Application.Features.Contabilidad.AsientosContables.Commands.ActivarAsientoContable;

public record ActivarAsientoContableCommand : IRequest<Unit>
{
    public Guid AsientoId { get; init; }
}