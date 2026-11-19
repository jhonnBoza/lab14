using Gimnasio.Entidades;

namespace Gimnasio.Datos.Repositorios;

public interface IPlanRepositorio
{
    Task<List<Plan>> ListarAsync(decimal? precioMax, int? duracionMeses, string? nombre);

    Task<Plan?> ObtenerAsync(int id);

    Task<bool> ExisteAsync(int id);

    Task<bool> ExisteNombreAsync(string nombre, int? exceptoId = null);

    Task<bool> TieneMiembrosAsync(int id);

    Task<List<Miembro>> ListarMiembrosAsync(int planId);

    Task AgregarAsync(Plan plan);

    Task<bool> ActualizarAsync(Plan datos);

    Task<bool> EliminarAsync(int id);

    Task GuardarAsync();
}
