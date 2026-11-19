using AutoMapper;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Entidades;
using Gimnasio.Entidades.Dtos;
using Gimnasio.Entidades.Excepciones;

namespace Gimnasio.Negocio.Servicios;

public class MiembroServicio(IMiembroRepositorio miembros, IPlanRepositorio planes, IMapper mapper) : IMiembroServicio
{
    public async Task<IReadOnlyList<MiembroDto>> ListarAsync(string? apellidos, int? planId, string? email)
    {
        var lista = await miembros.ListarAsync(apellidos, planId, email);
        return mapper.Map<List<MiembroDto>>(lista);
    }

    public async Task<MiembroDto> ObtenerAsync(int id)
    {
        var miembro = await miembros.ObtenerAsync(id)
            ?? throw new ReglaNegocioException("No encontré a ese miembro.", 404);
        return mapper.Map<MiembroDto>(miembro);
    }

    public async Task<MiembroDto> CrearAsync(MiembroDto dto)
    {
        Preparar(dto);
        if (!await planes.ExisteAsync(dto.PlanId))
        {
            throw new ReglaNegocioException("El plan indicado no existe.", 400);
        }

        if (await miembros.ExisteEmailAsync(dto.Email))
        {
            throw new ReglaNegocioException("Ya existe un miembro con ese correo.", 409);
        }

        var miembro = mapper.Map<Miembro>(dto);
        miembro.MiembroId = 0;
        miembro.Plan = null;
        await miembros.AgregarAsync(miembro);
        await miembros.GuardarAsync();

        miembro.Plan = null;
        var dtoCreado = mapper.Map<MiembroDto>(miembro);
        dtoCreado.NombrePlan = null;
        return dtoCreado;
    }

    public async Task ActualizarAsync(int id, MiembroDto dto)
    {
        Preparar(dto);
        if (!await planes.ExisteAsync(dto.PlanId))
        {
            throw new ReglaNegocioException("El plan indicado no existe.", 400);
        }

        if (await miembros.ExisteEmailAsync(dto.Email, id))
        {
            throw new ReglaNegocioException("Ya existe un miembro con ese correo.", 409);
        }

        var miembro = mapper.Map<Miembro>(dto);
        miembro.MiembroId = id;
        var actualizado = await miembros.ActualizarAsync(miembro);
        if (!actualizado)
        {
            throw new ReglaNegocioException("No encontré a ese miembro.", 404);
        }

        await miembros.GuardarAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var eliminado = await miembros.EliminarAsync(id);
        if (!eliminado)
        {
            throw new ReglaNegocioException("No encontré a ese miembro.", 404);
        }

        await miembros.GuardarAsync();
    }

    private static void Preparar(MiembroDto dto)
    {
        if (dto.FechaInscripcion == default)
        {
            throw new ReglaNegocioException("La fecha de inscripción es obligatoria.", 400);
        }

        dto.Nombres = dto.Nombres.Trim();
        dto.Apellidos = dto.Apellidos.Trim();
        dto.Email = dto.Email.Trim().ToLowerInvariant();
        dto.Telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono.Trim();
    }
}
