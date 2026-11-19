using Gimnasio.Entidades;
using Gimnasio.Entidades.Excepciones;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Datos.Repositorios;

public class PlanRepositorio(GimnasioDbContext db) : IPlanRepositorio
{
    public async Task<List<Plan>> ListarAsync(decimal? precioMax, int? duracionMeses, string? nombre)
    {
        var consulta = db.Planes.AsNoTracking().AsQueryable();

        if (precioMax is not null)
        {
            consulta = consulta.Where(p => p.PrecioMensual <= precioMax);
        }

        if (duracionMeses is not null)
        {
            consulta = consulta.Where(p => p.DuracionMeses == duracionMeses);
        }

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            consulta = consulta.Where(p => p.Nombre.Contains(nombre));
        }

        return await consulta.OrderBy(p => p.PlanId).ToListAsync();
    }

    public Task<Plan?> ObtenerAsync(int id) =>
        db.Planes.AsNoTracking().FirstOrDefaultAsync(p => p.PlanId == id);

    public Task<bool> ExisteAsync(int id) =>
        db.Planes.AnyAsync(p => p.PlanId == id);

    public Task<bool> ExisteNombreAsync(string nombre, int? exceptoId = null) =>
        db.Planes.AnyAsync(p =>
            p.Nombre.ToLower() == nombre.ToLower() &&
            (exceptoId == null || p.PlanId != exceptoId));

    public Task<bool> TieneMiembrosAsync(int id) =>
        db.Miembros.AnyAsync(m => m.PlanId == id);

    public Task<List<Miembro>> ListarMiembrosAsync(int planId) =>
        db.Miembros.AsNoTracking()
            .Include(m => m.Plan)
            .Where(m => m.PlanId == planId)
            .OrderBy(m => m.Apellidos)
            .ToListAsync();

    public async Task AgregarAsync(Plan plan) =>
        await db.Planes.AddAsync(plan);

    public async Task<bool> ActualizarAsync(Plan datos)
    {
        var actual = await db.Planes.FirstOrDefaultAsync(p => p.PlanId == datos.PlanId);
        if (actual is null)
        {
            return false;
        }

        actual.Nombre = datos.Nombre;
        actual.PrecioMensual = datos.PrecioMensual;
        actual.DuracionMeses = datos.DuracionMeses;
        actual.Descripcion = datos.Descripcion;
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var plan = await db.Planes.FirstOrDefaultAsync(p => p.PlanId == id);
        if (plan is null)
        {
            return false;
        }

        db.Planes.Remove(plan);
        return true;
    }

    public async Task GuardarAsync()
    {
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new ReglaNegocioException("Ya existe un plan con ese nombre.", 409);
        }
    }
}
