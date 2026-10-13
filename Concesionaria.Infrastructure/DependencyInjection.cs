using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Application.Interfaces;
using Concesionaria.Domain.Identity;
using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Identity;
using Concesionaria.Infrastructure.Persistence;
using Concesionaria.Infrastructure.Services;
using Concesionaria.Infrastructure.Services.Almacenamiento;
using Concesionaria.Infrastructure.Services.Imagenes;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Concesionaria.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // =========================
        // Base de Datos
        // =========================

        services.AddDbContext<ApplicationDbContext>(
            (serviceProvider, options) =>
            {
                var currentUser = serviceProvider.GetRequiredService<ICurrentUserService>();

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        );

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>()
        );

        // =========================
        // Identity
        // =========================

        services
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // =========================
        // HttpContext
        // =========================

        services.AddHttpContextAccessor();

        // =========================
        // Servicios
        // =========================

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<ILocalidadService, LocalidadService>();

        services.AddScoped<ISmsService, SmsService>();

        services.AddScoped<INotificationService, NotificationService>();

        services.Configure<ImageProcessingOptions>(
            configuration.GetSection(ImageProcessingOptions.SectionName));

        services.Configure<S3StorageOptions>(
            configuration.GetSection(S3StorageOptions.SectionName));

        services.Configure<LocalStorageOptions>(
            configuration.GetSection(LocalStorageOptions.SectionName));

        var storageProvider = configuration["FileStorage:Provider"] ?? "Local";

        if (storageProvider.Equals("Local", StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<IFileStorage, LocalFileStorage>();
        }
        else if (storageProvider.Equals("S3", StringComparison.OrdinalIgnoreCase))
        {
            services.AddSingleton<IAmazonS3>(serviceProvider =>
            {
                var options = serviceProvider
                    .GetRequiredService<IOptions<S3StorageOptions>>()
                    .Value;

                var clientConfiguration = new AmazonS3Config
                {
                    ForcePathStyle = options.ForcePathStyle
                };

                if (!string.IsNullOrWhiteSpace(options.ServiceUrl))
                {
                    clientConfiguration.ServiceURL = options.ServiceUrl;
                    clientConfiguration.AuthenticationRegion = options.Region;
                }
                else
                {
                    clientConfiguration.RegionEndpoint =
                        RegionEndpoint.GetBySystemName(options.Region);
                }

                var hasAccessKey = !string.IsNullOrWhiteSpace(options.AccessKey);
                var hasSecretKey = !string.IsNullOrWhiteSpace(options.SecretKey);

                if (hasAccessKey != hasSecretKey)
                {
                    throw new InvalidOperationException(
                        "S3Storage:AccessKey y S3Storage:SecretKey deben configurarse juntos.");
                }

                if (hasAccessKey)
                {
                    var credentials = new BasicAWSCredentials(
                        options.AccessKey,
                        options.SecretKey);

                    return new AmazonS3Client(credentials, clientConfiguration);
                }

                return new AmazonS3Client(clientConfiguration);
            });

            services.AddScoped<IFileStorage, S3FileStorage>();
        }
        else
        {
            throw new InvalidOperationException(
                $"El proveedor de archivos '{storageProvider}' no está soportado.");
        }

        services.AddScoped<IImageProcessor, MagickImageProcessor>();

        // =========================
        // Repositories
        // =========================

        services.AddScoped<ICategoriaGastoRepository, CategoriaGastoRepository>();

        services.AddScoped<ISucursalRepository, SucursalRepository>();

        services.AddScoped<ICuentaRepository, CuentaRepository>();
        
        services.AddScoped<IAsientoContableRepository, AsientoContableRepository>();

        //services.AddScoped<IAsientoDetalleRepository, AsientoDetalleRepository>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        services.AddScoped<IMarcaVehiculoRepository, MarcaVehiculoRepository>();

        services.AddScoped<ITipoVehiculoRepository, TipoVehiculoRepository>();

        services.AddScoped<IModeloVehiculoRepository, ModeloVehiculoRepository>();
        
        services.AddScoped<IVehiculoRepository, VehiculoRepository>();
        
        services.AddScoped<IProveedorRepository, ProveedorRepository>();
        
        services.AddScoped<IClienteRepository, ClienteRepository>();

        services.AddScoped<IVendedorRepository, VendedorRepository>();

        return services;
    }
}
