using MediatR;
namespace Application.Features.Personas.Commands.ActivarVendedor;

public record ActivarVendedorCommand : IRequest<Unit>{
    public Guid VendedorId { get; init; }
    public bool Eliminado { get; init; }
}
