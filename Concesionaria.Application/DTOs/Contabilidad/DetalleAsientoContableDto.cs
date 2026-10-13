public class DetalleAsientoContableDto
{
    public Guid Id { get; set; }
    public Guid CuentaId { get; set; }
    public string CuentaCodigo { get; set; }
    public string CuentaNombre { get; set; }
    public decimal Debe { get; set; }
    public decimal Haber { get; set; }
    public string Observacion { get; set; }
}