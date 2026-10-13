using MediatR;

namespace Application.Features.Personas.Commands.ActualizarVendedor;

public record ActualizarVendedorCommand : IRequest<Unit>
{
    public Guid VendedorId {get; init;}
    public string NombreCompleto { get; init; } = string.Empty;
    public string Dni { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public decimal ComisionPorcentaje { get; init; }
    public Guid LocalidadId { get; init; }
    public Guid SucursalId { get; init; }
}
