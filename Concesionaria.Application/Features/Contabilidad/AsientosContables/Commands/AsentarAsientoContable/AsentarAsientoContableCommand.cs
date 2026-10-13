using MediatR;

namespace Application.Features.Contabilidad.AsientosContables.Commands.AsentarAsientoContable;

public record AsentarAsientoContableCommand : IRequest<Unit>
{
    public Guid AsientoId { get; init; }
}