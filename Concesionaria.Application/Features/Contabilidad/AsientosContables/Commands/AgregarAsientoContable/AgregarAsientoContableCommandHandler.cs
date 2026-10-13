using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.AsientosContables;
using MediatR;

namespace Application.Features.Contabilidad.AsientosContables.Commands.AgregarAsientoContable;

public class AgregarAsientoContableCommandHandler
    : IRequestHandler<AgregarAsientoContableCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AgregarAsientoContableCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        AgregarAsientoContableCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var asiento = AsientoContable.Crear(
            empresaId,
            request.Fecha,
            request.Concepto,
            request.Origen
        );

        if (request.Detalles != null && request.Detalles.Count > 0)
        {
            foreach (var item in request.Detalles)
            {
                asiento.AgregarDetalle(item.CuentaId, item.Debe, item.Haber, item.Observacion);
            }
        }

        // AUDITORÍA
        // asiento.CreatedAt = DateTime.UtcNow;
        // asiento.CreatedBy = _currentUser.UserId;

        await _context.AsientosContables.AddAsync(asiento, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return asiento.Id;
    }
}