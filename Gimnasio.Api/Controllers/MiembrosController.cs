using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gimnasio.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MiembrosController(IMiembroServicio miembros) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MiembroDto>>> Get(
        [FromQuery] string? apellidos,
        [FromQuery] int? planId,
        [FromQuery] string? email)
    {
        var lista = await miembros.ListarAsync(apellidos, planId, email);
        return Ok(lista);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MiembroDto>> GetById(int id)
    {
        var miembro = await miembros.ObtenerAsync(id);
        return Ok(miembro);
    }

    [Authorize(Roles = RolesNegocio.Recepcionista)]
    [HttpPost]
    public async Task<ActionResult<MiembroDto>> Post(MiembroDto dto)
    {
        var creado = await miembros.CrearAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = creado.MiembroId }, creado);
    }

    [Authorize(Roles = RolesNegocio.Recepcionista)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, MiembroDto dto)
    {
        if (id != dto.MiembroId)
        {
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el del cuerpo." });
        }

        await miembros.ActualizarAsync(id, dto);
        return NoContent();
    }

    [Authorize(Roles = RolesNegocio.Recepcionista)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await miembros.EliminarAsync(id);
        return NoContent();
    }
}
