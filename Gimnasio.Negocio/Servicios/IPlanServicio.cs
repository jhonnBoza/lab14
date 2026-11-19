using Gimnasio.Entidades.Dtos;

namespace Gimnasio.Negocio.Servicios;

public interface IPlanServicio
{
    Task<IReadOnlyList<PlanDto>> ListarAsync(decimal? precioMax, int? duracionMeses, string? nombre);

    Task<PlanDto> ObtenerAsync(int id);

    Task<IReadOnlyList<MiembroDto>> ListarMiembrosAsync(int planId);

    Task<PlanDto> CrearAsync(PlanDto dto);

    Task ActualizarAsync(int id, PlanDto dto);

    Task EliminarAsync(int id);
}
