using Concesionaria.Domain.AsientosContables;
using Concesionaria.Domain.Contabilidad.AsientosContables.Enum;
using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class AsientoContableRepository : IAsientoContableRepository
{
    private readonly ApplicationDbContext _context;

    public AsientoContableRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // METODO PARA AGREGAR
    public async Task AddAsync(AsientoContable asiento)
    {
        await _context.AsientosContables.AddAsync(asiento);
    }

    // METODO PARA ACTUALIZAR
    public async Task UpdateAsync(AsientoContable asiento)
    {
        _context.AsientosContables.Update(asiento);
        await _context.SaveChangesAsync();
    }

    // METODO PARA OBTENER POR ID CON SUS LINEAS DE DETALLE Y CUENTAS
    public async Task<AsientoContable?> ObtenerPorIdConDetalleAsync(Guid id, Guid empresaId)
    {
        return await _context.AsientosContables
            .Include(a => a.Detalles)
                .ThenInclude(d => d.Cuenta)
            .FirstOrDefaultAsync(a => a.Id == id && a.EmpresaId == empresaId && !a.Eliminado);
    }

    // METODO PARA OBTENER ACTIVOS SEGUN FILTRO
    public async Task<List<AsientoContable>> ObtenerActivosAsync(
        Guid empresaId,
        DateTime? desde = null,
        DateTime? hasta = null,
        EstadoAsiento? estado = null,
        OrigenAsiento? origen = null)
    {
        var obtenerAsientosActivos = _context.AsientosContables
            .Include(a => a.Detalles)
                .ThenInclude(d => d.Cuenta)
            .Where(a => a.EmpresaId == empresaId)
            .AsQueryable();

        if (desde.HasValue)
            obtenerAsientosActivos = obtenerAsientosActivos.Where(a => a.Fecha >= desde.Value);

        if (hasta.HasValue)
            obtenerAsientosActivos = obtenerAsientosActivos.Where(a => a.Fecha <= hasta.Value);

        if (estado.HasValue)
            obtenerAsientosActivos = obtenerAsientosActivos.Where(a => a.Estado == estado.Value);

        if (origen.HasValue)
            obtenerAsientosActivos = obtenerAsientosActivos.Where(a => a.Origen == origen.Value);

        return await obtenerAsientosActivos.OrderByDescending(a => a.Fecha).ToListAsync();
    }

    // METODO PARA OBTENER INACTIVOS SEGUN FILTRO
    public async Task<List<AsientoContable>> ObtenerInactivosAsync(
        Guid empresaId,
        DateTime? desde = null,
        DateTime? hasta = null)
    {
        var obtenerAsientosInactivos = _context.AsientosContables
            .IgnoreQueryFilters()
            .Include(a => a.Detalles)
                .ThenInclude(d => d.Cuenta)
            .Where(a => a.EmpresaId == empresaId && a.Eliminado)
            .AsQueryable();

        if (desde.HasValue)
            obtenerAsientosInactivos = obtenerAsientosInactivos.Where(a => a.Fecha >= desde.Value);

        if (hasta.HasValue)
            obtenerAsientosInactivos = obtenerAsientosInactivos.Where(a => a.Fecha <= hasta.Value);

        return await obtenerAsientosInactivos.OrderByDescending(a => a.Fecha).ToListAsync();
    }
}