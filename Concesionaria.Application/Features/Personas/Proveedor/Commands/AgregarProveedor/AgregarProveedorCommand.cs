using MediatR;

public class AgregarProveedorCommand : IRequest<Guid>
{
    public string Nombre { get; init; }= string.Empty;
    public string Cuil { get; init; }= string.Empty;
    public string Telefono { get; init; }= string.Empty;
    public string Email { get; init; }= string.Empty;
    public string Domicilio { get; init; }= string.Empty;
    public string Servicio { get; init; }= string.Empty;
    public string Observacion { get; init; }= string.Empty;
    public Guid LocalidadId { get; init; }
}
