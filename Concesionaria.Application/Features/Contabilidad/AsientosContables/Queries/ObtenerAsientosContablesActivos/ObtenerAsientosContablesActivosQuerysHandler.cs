using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.Contabilidad.AsientosContables.Queries.ObtenerAsientosContablesActivos;

public class ObtenerAsientosContablesActivosQueryHandler
    : IRequestHandler<ObtenerAsientosContablesActivosQuery, List<AsientoContableDto>>
{
    private readonly IAsientoContableRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerAsientosContablesActivosQueryHandler(
        IAsientoContableRepository repository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<AsientoContableDto>> Handle(
        ObtenerAsientosContablesActivosQuery request,
        CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var asientos = await _repository.ObtenerActivosAsync(
            empresaId,
            request.Desde,
            request.Hasta,
            request.Estado,
            request.Origen);

        return asientos
            .Select(a => new AsientoContableDto
            {
                AsientoId = a.Id,
                Fecha = a.Fecha,
                Concepto = a.Concepto,
                Origen = a.Origen,
                Estado = a.Estado,
                TotalDebe = a.Detalles.Sum(d => d.Debe),
                TotalHaber = a.Detalles.Sum(d => d.Haber),
                Detalles = a.Detalles.Select(d => new DetalleAsientoContableDto
                {
                    Id = d.Id,
                    CuentaId = d.CuentaId,
                    CuentaNombre = d.Cuenta?.Nombre ?? string.Empty,
                    CuentaCodigo = d.Cuenta?.Codigo ?? string.Empty,
                    Debe = d.Debe,
                    Haber = d.Haber,
                    Observacion = d.Observacion
                }).ToList()
            })
            .OrderByDescending(a => a.Fecha)
            .ToList();
    }
}