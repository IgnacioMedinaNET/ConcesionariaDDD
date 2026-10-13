using Concesionaria.Domain.Contabilidad.AsientosContables.Enum;

public class AsientoContableDto
{
    public Guid AsientoId { get; set; }
    public DateTime Fecha { get; set; }
    public string Concepto { get; set; }
    public OrigenAsiento Origen { get; set; }
    public EstadoAsiento Estado { get; set; }
    public decimal TotalDebe { get; set; }
    public decimal TotalHaber { get; set; }
    public List<DetalleAsientoContableDto> Detalles { get; set; } = new();
}