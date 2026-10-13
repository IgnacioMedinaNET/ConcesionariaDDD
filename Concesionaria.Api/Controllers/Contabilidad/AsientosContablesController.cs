using Application.Features.Contabilidad.AsientosContables.Commands.ActivarAsientoContable;
using Application.Features.Contabilidad.AsientosContables.Commands.ActualizarAsientoContable;
using Application.Features.Contabilidad.AsientosContables.Commands.AgregarAsientoContable;
using Application.Features.Contabilidad.AsientosContables.Commands.DesactivarAsientoContable;
using Application.Features.Contabilidad.AsientosContables.Queries.ObtenerAsientoContablePorId;
using Application.Features.Contabilidad.AsientosContables.Queries.ObtenerAsientosContablesActivos;
using Application.Features.Contabilidad.AsientosContables.Queries.ObtenerAsientosContablesInactivos;
using Concesionaria.Domain.Contabilidad.AsientosContables.Enum;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AsientosContablesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AsientosContablesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO OBTENER ACTIVOS
    [HttpGet("activos")]
    public async Task<IActionResult> ObtenerActivos(
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta,
        [FromQuery] EstadoAsiento? estado,
        [FromQuery] OrigenAsiento? origen)
    {
        var resultadoAsientosActivos = await _mediator.Send(
            new ObtenerAsientosContablesActivosQuery
            {
                Desde = desde,
                Hasta = hasta,
                Estado = estado,
                Origen = origen
            }
        );

        return Ok(resultadoAsientosActivos);
    }

    // METODO PARA OBTENER INACTIVOS
    [HttpGet("inactivos")]
    public async Task<IActionResult> ObtenerInactivos(
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta)
    {
        var resultadoAsientosInactivos = await _mediator.Send(
            new ObtenerAsientosContablesInactivosQuery
            {
                Desde = desde,
                Hasta = hasta
            }
        );

        return Ok(resultadoAsientosInactivos);
    }

    // METODO PARA OBTENER POR ID
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(Guid id)
    {
        var asiento = await _mediator.Send(
            new ObtenerAsientoContablePorIdQuery { AsientoId = id }
        );

        if (asiento == null)
        {
            return NotFound(new { mensaje = "El asiento contable no existe." });
        }

        return Ok(asiento);
    }

    // METODO PARA AGREGAR ASIENTO CONTABLE
    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarAsientoContableCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(
            new { mensaje = "Asiento contable agregado correctamente.", asientoId = id }
        );
    }

    // METODO ACTUALIZAR
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarAsientoContableCommand command
    )
    {
        if (id != command.AsientoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }

        await _mediator.Send(command);

        return Ok(new { mensaje = "Asiento contable actualizado correctamente." });
    }

    // METODO ACTUALIZAR ESTADO A ACTIVAR
    [HttpPut("activar/{id}")]
    public async Task<IActionResult> Activar(
        Guid id,
        [FromBody] ActivarAsientoContableCommand command
    )
    {
        if (id != command.AsientoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }

        await _mediator.Send(command);

        return Ok(new { mensaje = "Asiento contable activado correctamente." });
    }

    // METODO ACTUALIZAR ESTADO A DESACTIVAR
    [HttpPut("desactivar/{id}")]
    public async Task<IActionResult> Desactivar(
        Guid id,
        [FromBody] DesactivarAsientoContableCommand command
    )
    {
        if (id != command.AsientoId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }

        await _mediator.Send(command);

        return Ok(new { mensaje = "Asiento contable desactivado correctamente." });
    }
}