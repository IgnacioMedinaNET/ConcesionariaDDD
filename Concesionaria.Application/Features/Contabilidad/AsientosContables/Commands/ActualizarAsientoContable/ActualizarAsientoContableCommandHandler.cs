// using Concesionaria.Application.Common.Interfaces;
// using MediatR;
// using Microsoft.EntityFrameworkCore;

// namespace Application.Features.Contabilidad.AsientosContables.Commands.ActualizarAsientoContable;

// public class ActualizarAsientoContableCommandHandler
//     : IRequestHandler<ActualizarAsientoContableCommand, Unit>
// {
//     private readonly IApplicationDbContext _context;
//     private readonly ICurrentUserService _currentUser;

//     public ActualizarAsientoContableCommandHandler(
//         IApplicationDbContext context,
//         ICurrentUserService currentUser
//     )
//     {
//         _context = context;
//         _currentUser = currentUser;
//     }

//     public async Task<Unit> Handle(
//         ActualizarAsientoContableCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         var empresaId = _currentUser.EmpresaId;

//         var asiento = await _context.AsientosContables
//             .Include(a => a.Detalles)
//             .FirstOrDefaultAsync(
//                 a => a.Id == request.AsientoId && a.EmpresaId == empresaId && !a.Eliminado,
//                 cancellationToken
//             );

//         if (asiento == null)
//             throw new Exception("El asiento contable no fue encontrado o se encuentra desactivado.");

//         // Actualizar encabezado
//         //asiento.ActualizarEncabezado(request.Fecha, request.Concepto);

//         // Reemplazar o actualizar los detalles
//         // if (request.Detalles != null)
//         // {
//         //     // Remover los detalles actuales
//         //     _context.DetallesAsientoContable.RemoveRange(asiento.Detalles);
//         //     asiento.LimpiarDetalles();

//         //     // Agregar las nuevas líneas
//         //     foreach (var item in request.Detalles)
//         //     {
//         //         asiento.AgregarDetalle(item.CuentaId, item.Debe, item.Haber, item.Observacion);
//         //     }
//         // }

//         // Reemplazar o actualizar los detalles
//         if (request.Detalles != null)
//         {
//             var detallesViejos = asiento.Detalles.ToList();

//             // 2. Si tenía detalles, los marcamos explícitamente para borrado en el DbContext
//             if (detallesViejos.Any())
//             {
//                 _context.DetallesAsientoContable.RemoveRange(detallesViejos);
//             }

//             // 3. Vaciamos la lista interna del agregado
//             asiento.LimpiarDetalles();

//             // 4. Actualizamos el encabezado del asiento
//             asiento.ActualizarEncabezado(request.Fecha, request.Concepto);

//             // 5. Agregamos las nuevas líneas
//             foreach (var item in request.Detalles)
//             {
//                 asiento.AgregarDetalle(item.CuentaId, item.Debe, item.Haber, item.Observacion ?? string.Empty);
//             }
//         }

//         // AUDITORÍA
//         // asiento.UpdatedAt = DateTime.UtcNow;
//         // asiento.UpdatedBy = _currentUser.UserId;

//         await _context.SaveChangesAsync(cancellationToken);
//         return Unit.Value;
//     }
// }


using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Contabilidad.AsientosContables.Commands.ActualizarAsientoContable;

public class ActualizarAsientoContableCommandHandler
    : IRequestHandler<ActualizarAsientoContableCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActualizarAsientoContableCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActualizarAsientoContableCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var asiento = await _context.AsientosContables
            .Include(a => a.Detalles)
            .FirstOrDefaultAsync(
                a => a.Id == request.AsientoId && a.EmpresaId == empresaId && !a.Eliminado,
                cancellationToken
            );

        if (asiento == null)
            throw new Exception("El asiento contable no fue encontrado o se encuentra desactivado.");

        // 1. Actualizar el encabezado siempre (fuera del if de detalles)
        asiento.ActualizarEncabezado(request.Fecha, request.Concepto);

        // 2. Limpiar los detalles anteriores de forma directa en la base de datos para evitar conflictos de tracking
        if (asiento.Detalles.Any())
        {
            
            _context.DetallesAsientoContable.RemoveRange(asiento.Detalles);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // 3. Vaciar la colección en memoria y agregar los nuevos
        asiento.LimpiarDetalles();

        if (request.Detalles != null)
        {
            foreach (var item in request.Detalles)
            {
                asiento.AgregarDetalle(item.CuentaId, item.Debe, item.Haber, item.Observacion ?? string.Empty);
            }
        }

        // AUDITORÍA
        // asiento.UpdatedAt = DateTime.UtcNow;
        // asiento.UpdatedBy = _currentUser.UserId;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}