using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gimnasio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanesController(IPlanServicio planes) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlanDto>>> Get(
        [FromQuery] decimal? precioMax,
        [FromQuery] int? duracionMeses,
        [FromQuery] string? nombre)
    {
        var lista = await planes.ListarAsync(precioMax, duracionMeses, nombre);
        return Ok(lista);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlanDto>> GetById(int id)
    {
        var plan = await planes.ObtenerAsync(id);
        return Ok(plan);
    }

    [Authorize]
    [HttpGet("{id:int}/miembros")]
    public async Task<ActionResult<IEnumerable<MiembroDto>>> GetMiembros(int id)
    {
        var miembros = await planes.ListarMiembrosAsync(id);
        return Ok(miembros);
    }

    [Authorize(Roles = RolesNegocio.Recepcionista)]
    [HttpPost]
    public async Task<ActionResult<PlanDto>> Post(PlanDto dto)
    {
        var creado = await planes.CrearAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = creado.PlanId }, creado);
    }

    [Authorize(Roles = RolesNegocio.Recepcionista)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, PlanDto dto)
    {
        if (id != dto.PlanId)
        {
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el del cuerpo." });
        }

        await planes.ActualizarAsync(id, dto);
        return NoContent();
    }

    [Authorize(Roles = RolesNegocio.Recepcionista)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await planes.EliminarAsync(id);
        return NoContent();
    }
}
