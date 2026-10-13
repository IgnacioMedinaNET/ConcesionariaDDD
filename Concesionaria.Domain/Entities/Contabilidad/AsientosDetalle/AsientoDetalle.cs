using Concesionaria.Domain.Common;
using Concesionaria.Domain.Cuentas;
using Concesionaria.Domain.AsientosContables;

namespace Concesionaria.Domain.AsientosDetalle;

public class DetalleAsientoContable : BaseEntity<Guid>
{
    public Guid AsientoContableId { get; private set; }
    public AsientoContable AsientoContable { get; private set; }

    public Guid CuentaId { get; private set; }
    public Cuenta Cuenta { get; private set; }

    public decimal Debe { get; private set; }
    public decimal Haber { get; private set; }
    public string Observacion { get; private set; }

    protected DetalleAsientoContable() { }

    internal DetalleAsientoContable(Guid asientoContableId, Guid cuentaId, decimal debe, decimal haber, string observacion = "")
    {
        Id = Guid.NewGuid();
        AsientoContableId = asientoContableId;
        CuentaId = cuentaId;
        Debe = debe;
        Haber = haber;
        Observacion = observacion?.Trim() ?? string.Empty;
    }

    internal void ModificarValores(Guid cuentaId, decimal debe, decimal haber, string observacion)
    {
        CuentaId = cuentaId;
        Debe = debe;
        Haber = haber;
        Observacion = observacion?.Trim() ?? string.Empty;
    }
}