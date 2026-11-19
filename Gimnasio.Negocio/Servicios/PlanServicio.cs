using AutoMapper;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;
using Gimnasio.Entidades.Excepciones;

namespace Gimnasio.Negocio.Servicios;

public class PlanServicio(IPlanRepositorio planes, IMapper mapper) : IPlanServicio
{
    public async Task<IReadOnlyList<PlanDto>> ListarAsync(decimal? precioMax, int? duracionMeses, string? nombre)
    {
        var lista = await planes.ListarAsync(precioMax, duracionMeses, nombre);
        return mapper.Map<List<PlanDto>>(lista);
    }

    public async Task<PlanDto> ObtenerAsync(int id)
    {
        var plan = await planes.ObtenerAsync(id)
            ?? throw new ReglaNegocioException("No encontré ese plan.", 404);
        return mapper.Map<PlanDto>(plan);
    }

    public async Task<IReadOnlyList<MiembroDto>> ListarMiembrosAsync(int planId)
    {
        if (!await planes.ExisteAsync(planId))
        {
            throw new ReglaNegocioException("No encontré ese plan.", 404);
        }

        var miembros = await planes.ListarMiembrosAsync(planId);
        return mapper.Map<List<MiembroDto>>(miembros);
    }

    public async Task<PlanDto> CrearAsync(PlanDto dto)
    {
        dto.Nombre = dto.Nombre.Trim();
        if (await planes.ExisteNombreAsync(dto.Nombre))
        {
            throw new ReglaNegocioException("Ya existe un plan con ese nombre.", 409);
        }

        var plan = mapper.Map<Plan>(dto);
        plan.PlanId = 0;
        plan.Miembros = [];
        await planes.AgregarAsync(plan);
        await planes.GuardarAsync();
        return mapper.Map<PlanDto>(plan);
    }

    public async Task ActualizarAsync(int id, PlanDto dto)
    {
        dto.Nombre = dto.Nombre.Trim();
        if (await planes.ExisteNombreAsync(dto.Nombre, id))
        {
            throw new ReglaNegocioException("Ya existe un plan con ese nombre.", 409);
        }

        var plan = mapper.Map<Plan>(dto);
        plan.PlanId = id;
        var actualizado = await planes.ActualizarAsync(plan);
        if (!actualizado)
        {
            throw new ReglaNegocioException("No encontré ese plan.", 404);
        }

        await planes.GuardarAsync();
    }

    public async Task EliminarAsync(int id)
    {
        if (await planes.TieneMiembrosAsync(id))
        {
            throw new ReglaNegocioException("No se puede eliminar el plan porque tiene miembros inscritos.", 409);
        }

        var eliminado = await planes.EliminarAsync(id);
        if (!eliminado)
        {
            throw new ReglaNegocioException("No encontré ese plan.", 404);
        }

        await planes.GuardarAsync();
    }
}
