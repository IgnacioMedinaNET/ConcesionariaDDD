using Microsoft.EntityFrameworkCore;
using Concesionaria.Domain.Identity;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Ubicaciones;
using Concesionaria.Domain.Cuentas;
using Concesionaria.Domain.Usuarios;
using System.Data;
using Concesionaria.Domain.AsientosContables;
using Concesionaria.Domain.AsientosDetalle;

namespace Concesionaria.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<Provincia> Provincias { get; }
    DbSet<Localidad> Localidades { get; }
    DbSet<Empresa> Empresas { get; }
    DbSet<CategoriaGasto> CategoriasGastos { get; }
    DbSet<Sucursal> Sucursales { get; }
    DbSet<Cuenta> Cuentas { get; }
    DbSet<Usuario> Usuarios { get; }
    DbSet<MarcaVehiculo> MarcasVehiculos { get; }
    DbSet<TipoVehiculo> TiposVehiculos { get; }
    DbSet<ModeloVehiculo> ModelosVehiculos { get; }
    DbSet<Vehiculo> Vehiculos { get; }
    DbSet<VehiculoImagen> VehiculoImagenes { get; }
    DbSet<Proveedor> Proveedores { get; }
    DbSet<Cliente> Clientes { get; }
    DbSet<Vendedor> Vendedores { get; }
    DbSet<AsientoContable> AsientosContables { get; }
    DbSet<DetalleAsientoContable> DetallesAsientoContable { get; }

    Task<IApplicationTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
