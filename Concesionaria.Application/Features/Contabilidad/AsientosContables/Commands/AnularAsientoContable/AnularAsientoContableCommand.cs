using MediatR;

namespace Application.Features.Contabilidad.AsientosContables.Commands.AnularAsientoContable;

public record AnularAsientoContableCommand : IRequest<Unit>
{
    public Guid AsientoId { get; init; }
}