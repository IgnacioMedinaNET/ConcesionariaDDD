using Concesionaria.Domain.Common;
using Concesionaria.Domain.Common.Interfaces;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Contabilidad.AsientosContables.Enum;
using Concesionaria.Domain.AsientosDetalle;

namespace Concesionaria.Domain.AsientosContables;

public class AsientoContable : BaseEntity<Guid>, ISoftDelete, IHasEmpresa, IAuditable
{
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; }

    public DateTime Fecha { get; private set; }
    public string Concepto { get; private set; }
    public OrigenAsiento Origen { get; private set; }
    public EstadoAsiento Estado { get; private set; }

    public bool Eliminado { get; set; } = false;

    // Reemplazás el campo privado y el ReadOnlyCollection por esto:
    public ICollection<DetalleAsientoContable> Detalles { get; private set; } = new List<DetalleAsientoContable>();
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;

    protected AsientoContable() { }

    private AsientoContable(Guid empresaId, DateTime fecha, string concepto, OrigenAsiento origen)
    {
        Id = Guid.NewGuid();
        EmpresaId = empresaId;
        Fecha = fecha;
        Concepto = concepto.Trim();
        Origen = origen;
        Estado = EstadoAsiento.BORRADOR;
        Eliminado = false;
    }

    public static AsientoContable Crear(Guid empresaId, DateTime fecha, string concepto, OrigenAsiento origen = OrigenAsiento.MANUAL)
    {
        return new AsientoContable(empresaId, fecha, concepto, origen);
    }

    public void ActualizarEncabezado(DateTime nuevaFecha, string nuevoConcepto)
    {
        Fecha = nuevaFecha;
        Concepto = nuevoConcepto.Trim();
    }

    public void ActualizarFecha(DateTime nuevaFecha)
    {
        Fecha = nuevaFecha;
    }

    public void ActualizarConcepto(string nuevoConcepto)
    {
        Concepto = nuevoConcepto.Trim();
    }

    public void AgregarDetalle(Guid cuentaId, decimal debe, decimal haber, string observacion = "")
    {
        Detalles.Add(new DetalleAsientoContable(Id, cuentaId, debe, haber, observacion));
    }

    public void LimpiarDetalles()
    {
        Detalles.Clear();
    }

    public void Confirmar() => Estado = EstadoAsiento.ASENTADO;
    public void Anular() => Estado = EstadoAsiento.ANULADO;
    public void Activar() => Eliminado = false;
    public void Desactivar() => Eliminado = true;
}