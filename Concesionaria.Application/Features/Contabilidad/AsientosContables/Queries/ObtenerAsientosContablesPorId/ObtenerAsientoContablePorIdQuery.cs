using MediatR;

namespace Application.Features.Contabilidad.AsientosContables.Queries.ObtenerAsientoContablePorId;

public record ObtenerAsientoContablePorIdQuery : IRequest<AsientoContableDto?>
{
    public Guid AsientoId { get; init; }
}