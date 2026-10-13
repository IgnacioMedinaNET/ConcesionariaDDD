using MediatR;

namespace Concesionaria.Application.Features.Personas.Vendedores.Queries.ObtenerVendedoresActivas;

public record ObtenerVendedoresActivasQuery : IRequest<List<VendedorDto>>
{
    public string Filtro { get; set; }
}