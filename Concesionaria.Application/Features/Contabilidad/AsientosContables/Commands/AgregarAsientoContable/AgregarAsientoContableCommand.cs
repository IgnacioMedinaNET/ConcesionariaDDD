using Concesionaria.Domain.Contabilidad.AsientosContables.Enum;
using MediatR;

namespace Application.Features.Contabilidad.AsientosContables.Commands.AgregarAsientoContable;

public record DetalleAsientoItemDto
{
    public Guid CuentaId { get; init; }
    public decimal Debe { get; init; }
    public decimal Haber { get; init; }
    public string Observacion { get; init; } = string.Empty;
}

public record AgregarAsientoContableCommand : IRequest<Guid>
{
    public DateTime Fecha { get; init; } = DateTime.Now;
    public string Concepto { get; init; } = string.Empty;
    public OrigenAsiento Origen { get; init; } = OrigenAsiento.MANUAL;
    public List<DetalleAsientoItemDto> Detalles { get; init; } = new();
}