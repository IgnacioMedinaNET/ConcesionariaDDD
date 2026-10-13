using MediatR;

namespace Concesionaria.Application.Features.Personas.Clientes.Queries.ObtenerClientesInactivas;

public record ObtenerClientesInactivasQuery : IRequest<List<ClienteDto>>
{
    public string Filtro { get; set; }
}