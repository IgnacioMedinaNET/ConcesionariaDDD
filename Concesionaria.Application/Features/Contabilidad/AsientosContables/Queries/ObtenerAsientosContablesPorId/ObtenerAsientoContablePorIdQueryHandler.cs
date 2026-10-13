using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.Contabilidad.AsientosContables.Queries.ObtenerAsientoContablePorId;

public class ObtenerAsientoContablePorIdQueryHandler
    : IRequestHandler<ObtenerAsientoContablePorIdQuery, AsientoContableDto?>
{
    private readonly IAsientoContableRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerAsientoContablePorIdQueryHandler(
        IAsientoContableRepository repository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<AsientoContableDto?> Handle(
        ObtenerAsientoContablePorIdQuery request,
        CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var asiento = await _repository.ObtenerPorIdConDetalleAsync(request.AsientoId, empresaId);

        if (asiento == null)
            return null;

        return new AsientoContableDto
        {
            AsientoId = asiento.Id,
            Fecha = asiento.Fecha,
            Concepto = asiento.Concepto,
            Origen = asiento.Origen,
            Estado = asiento.Estado,
            TotalDebe = asiento.Detalles.Sum(d => d.Debe),
            TotalHaber = asiento.Detalles.Sum(d => d.Haber),
            Detalles = asiento.Detalles.Select(d => new DetalleAsientoContableDto
            {
                Id = d.Id,
                CuentaId = d.CuentaId,
                CuentaNombre = d.Cuenta?.Nombre ?? string.Empty,
                CuentaCodigo = d.Cuenta?.Codigo ?? string.Empty,
                Debe = d.Debe,
                Haber = d.Haber,
                Observacion = d.Observacion
            }).ToList()
        };
    }
}