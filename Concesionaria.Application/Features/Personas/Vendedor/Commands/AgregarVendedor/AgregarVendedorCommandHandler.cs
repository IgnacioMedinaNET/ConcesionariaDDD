using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Personas.Commands.AgregarVendedor;

public class AgregarVendedorCommandHandler
    : IRequestHandler<AgregarVendedorCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AgregarVendedorCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        AgregarVendedorCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var vendedor = Vendedor.Crear(request.NombreCompleto, request.Dni, request.Email, request.ComisionPorcentaje, request.SucursalId, request.LocalidadId, empresaId);

        await _context.Vendedores.AddAsync(vendedor, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return vendedor.Id;
    }
}
