using Gimnasio.Datos.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gimnasio.Datos;

public static class Dependencias
{
    public static IServiceCollection AgregarDatos(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<GimnasioDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IPlanRepositorio, PlanRepositorio>();
        services.AddScoped<IMiembroRepositorio, MiembroRepositorio>();
        services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
        return services;
    }
}
