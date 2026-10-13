using MediatR;

namespace Application.Features.Personas.Commands.DesactivarVendedor;

public record DesactivarVendedorCommand : IRequest<Unit>{
    public Guid VendedorId { get; init; }
    public bool Eliminado { get; init; }
}
