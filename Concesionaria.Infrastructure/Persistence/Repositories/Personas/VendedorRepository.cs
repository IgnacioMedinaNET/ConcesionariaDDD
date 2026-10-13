using Concesionaria.Domain.Common.Enums;
using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class VendedorRepository : IVendedorRepository
{
    private readonly ApplicationDbContext _context;

    public VendedorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // METODO PARA AGREGAR
    public async Task AddAsync(Vendedor vendedor)
    {
        await _context.Vendedores.AddAsync(vendedor);
    }

    //METODO PARA ACTUALIZAR
    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    // METODO PARA OBTENER ACTIVAS SEGUN FILTRO
    public async Task<List<Vendedor>> ObtenerActivasAsync(Guid empresaId, string filtro = null)
    {
        var obtenerVendedoresActivos = _context
            .Vendedores.Where(v => v.EmpresaId == empresaId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var filtroNormalizado = filtro.Trim().ToLower();
            obtenerVendedoresActivos = obtenerVendedoresActivos.Where(v =>
                v.NombreCompleto.ToLower().Contains(filtroNormalizado)
                || v.Dni.ToLower().Contains(filtroNormalizado)
                || v.ComisionPorcentaje.ToString().Contains(filtroNormalizado)
                || v.Email.ToLower().Contains(filtroNormalizado)
                || v.Localidad.Nombre.ToLower().Contains(filtroNormalizado)
            );
        }

        return await obtenerVendedoresActivos.ToListAsync();
    }

    // METODO PARA OBTENER INACTIVAS SEGUN FILTRO
    public async Task<List<Vendedor>> ObtenerInactivasAsync(Guid empresaId, string filtro = null)
    {
        var obtenerVendedoresInactivos = _context
            .Vendedores.IgnoreQueryFilters()
            .Where(v => v.EmpresaId == empresaId && v.Eliminado)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var filtroNormalizado = filtro.Trim().ToLower();
            obtenerVendedoresInactivos = obtenerVendedoresInactivos.Where(v =>
                v.NombreCompleto.ToLower().Contains(filtroNormalizado)
                || v.Dni.ToLower().Contains(filtroNormalizado)
                || v.ComisionPorcentaje.ToString().Contains(filtroNormalizado)
                || v.Email.ToLower().Contains(filtroNormalizado)
                || v.Localidad.Nombre.ToLower().Contains(filtroNormalizado)
            );
        }

        return await obtenerVendedoresInactivos.ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA EN AGREGAR
    public async Task<EstadoExistencia> ExistePorDniAsync(string dni, Guid empresaId)
    {
        bool? estado = await _context
            .Vendedores.IgnoreQueryFilters()
            .Where(v => v.EmpresaId == empresaId && v.Dni == dni)
            .Select(v => (bool?)v.Eliminado)
            .FirstOrDefaultAsync();

        if (estado == null)
            return EstadoExistencia.NoExiste;

        return estado.Value ? EstadoExistencia.Desactivado : EstadoExistencia.Activo;
    }

    public async Task<EstadoExistencia> ExistePorEmailAsync(string email, Guid empresaId)
    {
        bool? estado = await _context
            .Vendedores.IgnoreQueryFilters()
            .Where(v => v.EmpresaId == empresaId && v.Email.ToLower() == email.ToLower().Trim())
            .Select(v => (bool?)v.Eliminado)
            .FirstOrDefaultAsync();

        if (estado == null)
            return EstadoExistencia.NoExiste;

        return estado.Value ? EstadoExistencia.Desactivado : EstadoExistencia.Activo;
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<EstadoExistencia> ExistePorDniExcluyendoIdAsync(
        string dni,
        Guid empresaId,
        Guid vendedorId
    )
    {
        bool? estado = await _context
            .Vendedores.IgnoreQueryFilters()
            .Where(v => v.EmpresaId == empresaId && v.Dni == dni && v.Id != vendedorId)
            .Select(v => (bool?)v.Eliminado)
            .FirstOrDefaultAsync();

        if (estado == null)
            return EstadoExistencia.NoExiste;

        return estado.Value ? EstadoExistencia.Desactivado : EstadoExistencia.Activo;
    }

    public async Task<EstadoExistencia> ExistePorEmailExcluyendoIdAsync(
        string email,
        Guid empresaId,
        Guid vendedorId
    )
    {
        bool? estado = await _context
            .Vendedores.IgnoreQueryFilters()
            .Where(v =>
                v.EmpresaId == empresaId
                && v.Email.ToLower() == email.ToLower().Trim()
                && v.Id != vendedorId
            )
            .Select(v => (bool?)v.Eliminado)
            .FirstOrDefaultAsync();

        if (estado == null)
            return EstadoExistencia.NoExiste;

        return estado.Value ? EstadoExistencia.Desactivado : EstadoExistencia.Activo;
    }
}
