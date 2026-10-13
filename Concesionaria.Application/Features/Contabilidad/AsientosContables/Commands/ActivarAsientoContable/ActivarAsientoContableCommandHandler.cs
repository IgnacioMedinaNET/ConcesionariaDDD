using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Contabilidad.AsientosContables.Commands.ActivarAsientoContable;

public class ActivarAsientoContableCommandHandler
    : IRequestHandler<ActivarAsientoContableCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActivarAsientoContableCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActivarAsientoContableCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        // Se usa IgnoreQueryFilters() por si tenés SoftDelete global en EF Core
        var asiento = await _context.AsientosContables
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(
                a => a.Id == request.AsientoId && a.EmpresaId == empresaId && a.Eliminado,
                cancellationToken
            );

        if (asiento == null)
            throw new Exception("El asiento contable no existe o ya se encuentra activo.");

        asiento.Activar();

        // AUDITORÍA
        // asiento.UpdatedAt = DateTime.UtcNow;
        // asiento.UpdatedBy = _currentUser.UserId;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}