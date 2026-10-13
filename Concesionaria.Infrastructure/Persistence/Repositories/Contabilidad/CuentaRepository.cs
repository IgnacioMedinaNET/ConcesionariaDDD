using Concesionaria.Domain.Cuentas;
using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Concesionaria.Domain.Cuentas.Enums;
using Concesionaria.Domain.Common.Enums;

namespace Infrastructure.Persistence.Repositories;

public class CuentaRepository : ICuentaRepository
{
    private readonly ApplicationDbContext _context;

    public CuentaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    //METODO PARA AGREGAR
    public async Task AddAsync(Cuenta cuenta)
    {
        await _context.Cuentas.AddAsync(cuenta);
    }

    //METODO PARA ACTUALIZAR
    public async Task UpdateAsync(Cuenta cuenta)
    {
        _context.Cuentas.Update(cuenta);
        await _context.SaveChangesAsync();
    }

    // METODO PARA OBTENER ACTIVAS SEGUN FILTRO
    public async Task<List<Cuenta>> ObtenerActivasAsync(
        Guid empresaId,
        string filtro = null,
        TipoCuenta? tipo = null,
        int? nivel = null)
    {
        var obtenerCuentasActivas = _context.Cuentas.Where(c => c.EmpresaId == empresaId).AsQueryable();
        if (!string.IsNullOrEmpty(filtro))
        {
            obtenerCuentasActivas = obtenerCuentasActivas.Where(c =>
                c.Codigo.Contains(filtro) ||
                c.Nombre.Contains(filtro));
        }

        if (tipo.HasValue)
            obtenerCuentasActivas = obtenerCuentasActivas.Where(c => c.Tipo == tipo.Value);

        if (nivel.HasValue)
            obtenerCuentasActivas = obtenerCuentasActivas.Where(c => c.Nivel == nivel.Value);

        return await obtenerCuentasActivas.ToListAsync();
    }

    // METODO PARA OBTENER INACTIVAS SEGUN FILTRO
    public async Task<List<Cuenta>> ObtenerInactivasAsync(
        Guid empresaId,
        string filtro = null,
        TipoCuenta? tipo = null,
        int? nivel = null)
    {
        var obtenerCuentasInactivas = _context.Cuentas
            .IgnoreQueryFilters()
            .Where(c => c.EmpresaId == empresaId && c.Eliminado)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filtro))
        {
            obtenerCuentasInactivas = obtenerCuentasInactivas.Where(c =>
                c.Codigo.Contains(filtro) ||
                c.Nombre.Contains(filtro));
        }

        if (tipo.HasValue)
            obtenerCuentasInactivas = obtenerCuentasInactivas.Where(c => c.Tipo == tipo.Value);

        if (nivel.HasValue)
            obtenerCuentasInactivas = obtenerCuentasInactivas.Where(c => c.Nivel == nivel.Value);

        return await obtenerCuentasInactivas.ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA POR NOMBRE PARA AGREGAR
    public async Task<EstadoExistencia> ExistePorNombreAsync(string nombre, Guid empresaId, TipoCuenta tipo)
    {
        var eliminado = await _context.Cuentas
            .IgnoreQueryFilters()
            .Where(c => c.EmpresaId == empresaId
                && c.Tipo == tipo
                && c.Nombre.ToLower() == nombre.Trim().ToLower())
            .Select(c => (bool?)c.Eliminado)
            .FirstOrDefaultAsync();

        return eliminado switch
        {
            null => EstadoExistencia.NoExiste,
            true => EstadoExistencia.Desactivado,
            _ => EstadoExistencia.Activo,
        };
    }

    // METODO PARA VALIDAR EXISTENCIA POR CODIGO PARA AGREGAR
    public async Task<EstadoExistencia> ExistePorCodigoAsync(string codigo, Guid empresaId)
    {
        var eliminado = await _context.Cuentas
            .IgnoreQueryFilters()
            .Where(c => c.EmpresaId == empresaId && c.Codigo.ToLower() == codigo.Trim().ToLower())
            .Select(c => (bool?)c.Eliminado)
            .FirstOrDefaultAsync();

        return eliminado switch
        {
            null => EstadoExistencia.NoExiste,
            true => EstadoExistencia.Desactivado,
            _ => EstadoExistencia.Activo,
        };
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<EstadoExistencia> ExistePorNombreExluyendoIdAsync(
     string nombre,
     Guid empresaId,
     Guid cuentaId)
    {
        var tipo = await _context.Cuentas
            .IgnoreQueryFilters()
            .Where(c => c.Id == cuentaId && c.EmpresaId == empresaId)
            .Select(c => (TipoCuenta?)c.Tipo)
            .FirstOrDefaultAsync();

        if (!tipo.HasValue)
            return EstadoExistencia.NoExiste;

        var eliminado = await _context.Cuentas
            .IgnoreQueryFilters()
            .Where(c => c.Nombre.ToLower() == nombre.Trim().ToLower()
                && c.EmpresaId == empresaId
                && c.Tipo == tipo.Value
                && c.Id != cuentaId)
            .Select(c => (bool?)c.Eliminado)
            .FirstOrDefaultAsync();

        return eliminado switch
        {
            null => EstadoExistencia.NoExiste,
            true => EstadoExistencia.Desactivado,
            _ => EstadoExistencia.Activo,
        };
    }

    public async Task<bool> TieneCuentasHijasActivasAsync(Guid empresaId, Guid cuentaId)
    {
        return await _context.Cuentas.AnyAsync(c =>
            c.EmpresaId == empresaId && c.CuentaPadreId == cuentaId);
    }

    public async Task<Cuenta?> ObtenerPorIdAsync(Guid id, Guid empresaId)
    {
        return await _context.Cuentas
            .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId && !c.Eliminado);
    }

}
