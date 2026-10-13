using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Personas.Commands.DesactivarVendedor;

public class DesactivarVendedorCommandHandler
    : IRequestHandler<DesactivarVendedorCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DesactivarVendedorCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        DesactivarVendedorCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        
        var obtenerVendedorId = await _context.Vendedores.IgnoreQueryFilters().FirstOrDefaultAsync(
            mv => mv.Id == request.VendedorId && mv.EmpresaId == empresaId && !mv.Eliminado,
            cancellationToken
        );

        if (obtenerVendedorId == null)
            throw new Exception("Vendedor no encontrado.");

        obtenerVendedorId.Desactivar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}