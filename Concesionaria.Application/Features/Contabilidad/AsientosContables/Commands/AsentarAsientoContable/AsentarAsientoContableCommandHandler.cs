using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Contabilidad.AsientosContables.Commands.AsentarAsientoContable;

public class AsentarAsientoContableCommandHandler
    : IRequestHandler<AsentarAsientoContableCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AsentarAsientoContableCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        AsentarAsientoContableCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var asiento = await _context.AsientosContables.FirstOrDefaultAsync(
            a => a.Id == request.AsientoId && a.EmpresaId == empresaId && !a.Eliminado,
            cancellationToken
        );

        if (asiento == null)
            throw new Exception("El asiento contable no existe o se encuentra desactivado.");

        asiento.Confirmar();

        // AUDITORÍA
        // asiento.UpdatedAt = DateTime.UtcNow;
        // asiento.UpdatedBy = _currentUser.UserId;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}