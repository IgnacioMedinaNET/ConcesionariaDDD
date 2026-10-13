using MediatR;

namespace Application.Features.Contabilidad.AsientosContables.Commands.DesactivarAsientoContable;

public record DesactivarAsientoContableCommand : IRequest<Unit>
{
    public Guid AsientoId { get; init; }
}