using MediatR;

namespace Concesionaria.Application.Features.Personas.Vendedores.Queries.ObtenerVendedoresInactivas;

public record ObtenerVendedoresInactivasQuery : IRequest<List<VendedorDto>>
{
    public string Filtro { get; set; }
}