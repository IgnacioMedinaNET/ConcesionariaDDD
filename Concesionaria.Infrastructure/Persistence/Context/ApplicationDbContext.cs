using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Common;
using Concesionaria.Domain.Common.Interfaces;
using Concesionaria.Domain.AsientosContables;
using Concesionaria.Domain.Cuentas;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Identity;
using Concesionaria.Domain.Ubicaciones;
using Concesionaria.Domain.Usuarios;
using Concesionaria.Infrastructure.Persistence.Configurations.Vehiculos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Concesionaria.Domain.AsientosDetalle;

namespace Concesionaria.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUser;

    private Guid CurrentEmpresaId => _currentUser.EmpresaId;
    private Guid CurrentSucursalId => _currentUser.SucursalId;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<Cuenta> Cuentas => Set<Cuenta>();
    public DbSet<CategoriaGasto> CategoriasGastos => Set<CategoriaGasto>();
    public DbSet<Sucursal> Sucursales => Set<Sucursal>();
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Provincia> Provincias => Set<Provincia>();
    public DbSet<Localidad> Localidades => Set<Localidad>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<MarcaVehiculo> MarcasVehiculos => Set<MarcaVehiculo>();
    public DbSet<TipoVehiculo> TiposVehiculos => Set<TipoVehiculo>();
    public DbSet<ModeloVehiculo> ModelosVehiculos => Set<ModeloVehiculo>();
    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
    public DbSet<VehiculoImagen> VehiculoImagenes => Set<VehiculoImagen>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Vendedor> Vendedores => Set<Vendedor>();
    public DbSet<AsientoContable> AsientosContables => Set<AsientoContable>();
    public DbSet<DetalleAsientoContable> DetallesAsientoContable => Set<DetalleAsientoContable>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Empresa>()
            .Property(e => e.MonedaPrincipal)
            .HasConversion<string>();

        builder.Entity<ApplicationUser>()
            .Property(u => u.RolId)
            .HasMaxLength(450);

        builder.ApplyConfiguration(new VehiculoImagenConfiguration());

        builder.Entity<ApplicationUser>()
            .HasOne(u => u.Empresa)
            .WithMany()
            .HasForeignKey(u => u.EmpresaId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ApplicationUser>()
            .HasOne<Sucursal>()
            .WithMany()
            .HasForeignKey(u => u.SucursalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Cuenta>(entity =>
        {
            entity.HasOne(c => c.CuentaPadre)
                .WithMany(c => c.CuentasHijas)
                .HasForeignKey(c => c.CuentaPadreId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");

            entity.Property(u => u.RolId)
                .IsRequired()
                .HasMaxLength(450);

            entity.Property(u => u.NombreCompleto)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Estado)
                .IsRequired()
                .HasConversion<string>();

            entity.HasIndex(u => new { u.EmpresaId, u.Email })
                .IsUnique();

            entity.HasOne(u => u.Empresa)
                .WithMany()
                .HasForeignKey(u => u.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Sucursal)
                .WithMany()
                .HasForeignKey(u => u.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<IdentityRole>()
                .WithMany()
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ModeloVehiculo>()
        .HasOne(m => m.MarcaVehiculo)
        .WithMany()
        .HasForeignKey(m => m.MarcaVehiculoId)
        .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ModeloVehiculo>()
        .HasOne(m => m.TipoVehiculo)
        .WithMany()
        .HasForeignKey(m => m.TipoVehiculoId)
        .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ModeloVehiculo>()
        .HasOne(m => m.Empresa)
        .WithMany()
        .HasForeignKey(m => m.EmpresaId)
        .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Cliente>(entity =>
        {

            entity.HasOne(m => m.Localidad)
            .WithMany()
            .HasForeignKey(m => m.LocalidadId)
            .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(m => m.Empresa)
            .WithMany()
            .HasForeignKey(m => m.EmpresaId)
            .OnDelete(DeleteBehavior.NoAction);

        });

        builder.Entity<Proveedor>()
        .HasOne(m => m.Localidad)
        .WithMany()
        .HasForeignKey(m => m.LocalidadId)
        .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Vehiculo>(entity =>
       {

           entity.HasOne(m => m.Empresa)
            .WithMany()
            .HasForeignKey(m => m.EmpresaId)
            .OnDelete(DeleteBehavior.NoAction);

           entity.HasOne(m => m.Modelo)
            .WithMany()
            .HasForeignKey(m => m.ModeloId)
            .OnDelete(DeleteBehavior.NoAction);

           entity.HasOne(m => m.Sucursal)
            .WithMany()
            .HasForeignKey(m => m.SucursalId)
            .OnDelete(DeleteBehavior.NoAction);

       });

        builder.Entity<Vendedor>(entity =>
       {

           entity.HasOne(m => m.Empresa)
            .WithMany()
            .HasForeignKey(m => m.EmpresaId)
            .OnDelete(DeleteBehavior.NoAction);

           entity.HasOne(m => m.Localidad)
            .WithMany()
            .HasForeignKey(m => m.LocalidadId)
            .OnDelete(DeleteBehavior.NoAction);

           entity.HasOne(m => m.Sucursal)
            .WithMany()
            .HasForeignKey(m => m.SucursalId)
            .OnDelete(DeleteBehavior.NoAction);
       });

        builder.Entity<AsientoContable>(entity =>
        {
            entity.ToTable("AsientosContables");
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Concepto)
                .IsRequired()
                .HasMaxLength(250);

            entity.HasMany(a => a.Detalles)
                .WithOne(d => d.AsientoContable)
                .HasForeignKey(d => d.AsientoContableId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<DetalleAsientoContable>(entity =>
        {
            entity.ToTable("DetallesAsientoContable");
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Id)
                .ValueGeneratedNever();

            entity.Property(d => d.Debe)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            entity.Property(d => d.Haber)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            entity.Property(d => d.Observacion)
                .HasMaxLength(500);

            entity.HasOne(d => d.Cuenta)
                .WithMany()
                .HasForeignKey(d => d.CuentaId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        ApplyGlobalFilters(builder);
    }

    private void ApplyGlobalFilters(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (!typeof(IHasEmpresa).IsAssignableFrom(entityType.ClrType))
                continue;

            var hasSucursal = typeof(IHasSucursal).IsAssignableFrom(entityType.ClrType);
            var hasSoftDelete = typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType);

            var filterMethodName = (hasSucursal, hasSoftDelete) switch
            {
                (true, true) => nameof(SetEmpresaSucursalSoftDeleteFilter),
                (true, false) => nameof(SetEmpresaSucursalFilter),
                (false, true) => nameof(SetEmpresaSoftDeleteFilter),
                _ => nameof(SetEmpresaFilter)
            };

            var filterMethod = typeof(ApplicationDbContext)
                .GetMethod(
                    filterMethodName,
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Static)!
                .MakeGenericMethod(entityType.ClrType);

            filterMethod.Invoke(null, new object[] { builder, this });
        }
    }

    private static void SetEmpresaFilter<TEntity>(
        ModelBuilder builder,
        ApplicationDbContext context)
        where TEntity : class, IHasEmpresa
    {
        builder.Entity<TEntity>()
            .HasQueryFilter(entity => entity.EmpresaId == context.CurrentEmpresaId);
    }

    private static void SetEmpresaSoftDeleteFilter<TEntity>(
        ModelBuilder builder,
        ApplicationDbContext context)
        where TEntity : class, IHasEmpresa, ISoftDelete
    {
        builder.Entity<TEntity>()
            .HasQueryFilter(entity =>
                entity.EmpresaId == context.CurrentEmpresaId &&
                !entity.Eliminado);
    }

    private static void SetEmpresaSucursalFilter<TEntity>(
        ModelBuilder builder,
        ApplicationDbContext context)
        where TEntity : class, IHasEmpresa, IHasSucursal
    {
        builder.Entity<TEntity>()
            .HasQueryFilter(entity =>
                entity.EmpresaId == context.CurrentEmpresaId &&
                entity.SucursalId == context.CurrentSucursalId);
    }

    private static void SetEmpresaSucursalSoftDeleteFilter<TEntity>(
        ModelBuilder builder,
        ApplicationDbContext context)
        where TEntity : class, IHasEmpresa, IHasSucursal, ISoftDelete
    {
        builder.Entity<TEntity>()
            .HasQueryFilter(entity =>
                entity.EmpresaId == context.CurrentEmpresaId &&
                entity.SucursalId == context.CurrentSucursalId &&
                !entity.Eliminado);
    }

    public async Task<IApplicationTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default)
    {
        var transaction = await Database.BeginTransactionAsync(
            isolationLevel,
            cancellationToken);

        return new ApplicationTransaction(transaction);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var entries = ChangeTracker.Entries<IAuditable>();

        var now = DateTime.UtcNow;
        var userId = _currentUser.UserId;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = userId;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = userId;
            }
        }
    }
}


