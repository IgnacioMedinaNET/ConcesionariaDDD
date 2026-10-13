using Concesionaria.Domain.Contabilidad.AsientosContables.Enum;
using MediatR;

namespace Application.Features.Contabilidad.AsientosContables.Queries.ObtenerAsientosContablesActivos;

public record ObtenerAsientosContablesActivosQuery : IRequest<List<AsientoContableDto>>
{
    public DateTime? Desde { get; init; }
    public DateTime? Hasta { get; init; }
    public EstadoAsiento? Estado { get; init; }
    public OrigenAsiento? Origen { get; init; }
}