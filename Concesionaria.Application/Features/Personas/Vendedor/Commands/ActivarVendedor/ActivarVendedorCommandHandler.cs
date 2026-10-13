using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Personas.Commands.ActivarVendedor;

public class ActivarVendedorCommandHandler
    : IRequestHandler<ActivarVendedorCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActivarVendedorCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActivarVendedorCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        
        var obtenerVendedorId = await _context.Vendedores.IgnoreQueryFilters().FirstOrDefaultAsync(
            m => m.Id == request.VendedorId && m.EmpresaId == empresaId && m.Eliminado,
            cancellationToken
        );

        if (obtenerVendedorId == null)
            throw new Exception("Vendedor no encontrado.");

        obtenerVendedorId.Activar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
