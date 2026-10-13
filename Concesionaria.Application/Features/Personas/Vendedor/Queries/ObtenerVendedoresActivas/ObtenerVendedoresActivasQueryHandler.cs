using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Concesionaria.Application.Features.Personas.Vendedores.Queries.ObtenerVendedoresActivas;

public class ObtenerVendedoresActivasQueryHandler
    : IRequestHandler<ObtenerVendedoresActivasQuery, List<VendedorDto>>
{
    private readonly IVendedorRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerVendedoresActivasQueryHandler(IVendedorRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<VendedorDto>> Handle(
        ObtenerVendedoresActivasQuery request,
        CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var vendedores = await _repository.ObtenerActivasAsync(empresaId, request.Filtro);

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
                SucursalId = c.SucursalId
            })
            .ToList();
    }
}
