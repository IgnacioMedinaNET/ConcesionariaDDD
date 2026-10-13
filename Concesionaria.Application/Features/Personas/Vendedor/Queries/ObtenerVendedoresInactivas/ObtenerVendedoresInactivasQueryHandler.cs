using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Concesionaria.Application.Features.Personas.Vendedores.Queries.ObtenerVendedoresInactivas;

public class ObtenerVendedoresInactivasQueryHandler
    : IRequestHandler<ObtenerVendedoresInactivasQuery, List<VendedorDto>>
{
    private readonly IVendedorRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerVendedoresInactivasQueryHandler(
        IVendedorRepository repository,
        ICurrentUserService currentUserService
    )
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<VendedorDto>> Handle(
        ObtenerVendedoresInactivasQuery request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUserService.EmpresaId;

        var vendedores = await _repository.ObtenerInactivasAsync(empresaId, request.Filtro);

        return vendedores
            .OrderBy(c => c.NombreCompleto)
            .Select(c => new VendedorDto
            {
                VendedorId = c.Id,
                Nombre = c.NombreCompleto,
                Dni = c.Dni,
                Email = c.Email,
                ComisionPorcentaje = c.ComisionPorcentaje,
                LocalidadId = c.LocalidadId,
                SucursalId = c.SucursalId,
            })
            .ToList();
    }
}
