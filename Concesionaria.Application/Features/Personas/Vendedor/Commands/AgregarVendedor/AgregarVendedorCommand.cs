using MediatR;

namespace Application.Features.Personas.Commands.AgregarVendedor;
public record AgregarVendedorCommand : IRequest<Guid>
{
    public string NombreCompleto { get; init; } = string.Empty;
    public string Dni { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public decimal ComisionPorcentaje { get; init; }
    public Guid LocalidadId { get; init; }
    public Guid SucursalId { get; init; }
}
