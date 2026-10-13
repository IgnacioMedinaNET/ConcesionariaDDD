using Concesionaria.Domain.Contabilidad.AsientosContables.Enum;
using MediatR;

namespace Application.Features.Contabilidad.AsientosContables.Commands.ActualizarAsientoContable;

public record DetalleAsientoActualizarDto
{
    public Guid CuentaId { get; init; }
    public decimal Debe { get; init; }
    public decimal Haber { get; init; }
    public string Observacion { get; init; } = string.Empty;
}

public record ActualizarAsientoContableCommand : IRequest<Unit>
{
    public Guid AsientoId { get; init; }
    public DateTime Fecha { get; init; }
    public string Concepto { get; init; } = string.Empty;
    public OrigenAsiento Origen { get; init; }
    public List<DetalleAsientoActualizarDto> Detalles { get; init; } = new();
}