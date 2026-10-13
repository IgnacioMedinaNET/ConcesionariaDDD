using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Concesionaria.Application.Features.Personas.Clientes.Queries.ObtenerClientesInactivas;

public class ObtenerClientesInactivasQueryHandler
    : IRequestHandler<ObtenerClientesInactivasQuery, List<ClienteDto>>
{
    private readonly IClienteRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerClientesInactivasQueryHandler(IClienteRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<ClienteDto>> Handle(
        ObtenerClientesInactivasQuery request,
        CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var clientes = await _repository.ObtenerInactivasAsync(empresaId, request.Filtro);

        return clientes
            .OrderBy(c => c.NombreCompleto)
            .Select(c => new ClienteDto
            {
                ClienteId = c.Id,
                NombreCompleto = c.NombreCompleto,
                Dni = c.Dni,
                Telefono = c.Telefono,
                Email = c.Email,
                Domicilio = c.Domicilio,
                LocalidadId = c.LocalidadId
            })
            .ToList();
    }
}
