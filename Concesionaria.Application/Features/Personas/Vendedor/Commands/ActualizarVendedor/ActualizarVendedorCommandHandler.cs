using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Personas.Commands.ActualizarVendedor;
public class ActualizarVendedorCommandHandler
    : IRequestHandler<ActualizarVendedorCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActualizarVendedorCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActualizarVendedorCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var obtenerVendedorId = await _context.Vendedores.FirstOrDefaultAsync(
            m => m.Id == request.VendedorId && m.SucursalId == request.SucursalId && m.EmpresaId == empresaId && !m.Eliminado,
            cancellationToken
        );

        if (obtenerVendedorId == null)
            throw new Exception("Vendedor no encontrado.");

        obtenerVendedorId.ActualizarVendedor(request.NombreCompleto, request.Dni, request.Email, request.ComisionPorcentaje, request.SucursalId, request.LocalidadId);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
