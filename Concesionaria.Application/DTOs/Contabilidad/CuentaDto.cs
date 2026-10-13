using Concesionaria.Domain.Cuentas.Enums;

public class CuentaDto
{
    public Guid CuentaId { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public TipoCuenta Tipo { get; set; }
    public int Nivel { get; set; }
    public Guid? CuentaPadreId { get; set; }
}
