using Application.Features.Personas.Commands.ActivarVendedor;
using Application.Features.Personas.Commands.ActualizarVendedor;
using Application.Features.Personas.Commands.AgregarVendedor;
using Application.Features.Personas.Commands.DesactivarVendedor;
using Concesionaria.Application.Features.Personas.Vendedores.Queries.ObtenerVendedoresActivas;
using Concesionaria.Application.Features.Personas.Vendedores.Queries.ObtenerVendedoresInactivas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VendedoresController : ControllerBase
{
    private readonly IMediator _mediator;

    public VendedoresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // METODO OBTENER ACTIVAS
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerActivas([FromQuery] string filtro)
    {
        var resultadoVendedoresActivas = await _mediator.Send(new ObtenerVendedoresActivasQuery
        {
            Filtro = filtro
        });

        return Ok(resultadoVendedoresActivas);
    }
    // METODO OBTENER INACTIVAS
    [HttpGet("inactivas")]
    public async Task<IActionResult> ObtenerInactivas([FromQuery] string filtro)
    {
        var resultadoVendedoresInactivas = await _mediator.Send(new ObtenerVendedoresInactivasQuery
        {
            Filtro = filtro
        });

        return Ok(resultadoVendedoresInactivas);
    }

     // METODO AGREGAR
    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarVendedorCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(
            new { mensaje = "Vendedor registrado correctamente.", vendedorId = id }
        );
    }

    // METODO ACTUALIZAR
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarVendedorCommand command
    )
    {
        if (id != command.VendedorId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        await _mediator.Send(command);

        return Ok(new { mensaje = "Vendedor actualizado correctamente." });
    }

     // METODO ACTUALIZAR ESTADO A DESACTIVAR
    [HttpPut("desactivar/{id}")]
    public async Task<IActionResult> Desactivar(
        Guid id,
        [FromBody] DesactivarVendedorCommand command
    )
    {
        if (id != command.VendedorId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Vendedor desactivado correctamente." });
    }

           // METODO ACTUALIZAR ESTADO A ACTIVAR
    [HttpPut("activar/{id}")]
    public async Task<IActionResult> Activar(
        Guid id,
        [FromBody] ActivarVendedorCommand command
    )
    {
        if (id != command.VendedorId)
        {
            return BadRequest(new { mensaje = "El Id no coincide." });
        }
        
        await _mediator.Send(command);

        return Ok(new { mensaje = "Vendedor activado correctamente." });
    }
}