using MediatR;

namespace Application.Features.Contabilidad.AsientosContables.Queries.ObtenerAsientosContablesInactivos;

public record ObtenerAsientosContablesInactivosQuery : IRequest<List<AsientoContableDto>>
{
    public DateTime? Desde { get; init; }
    public DateTime? Hasta { get; init; }
}