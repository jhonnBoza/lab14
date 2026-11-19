using Gimnasio.Negocio.Servicios;
using Microsoft.Extensions.DependencyInjection;

namespace Gimnasio.Negocio;

public static class Dependencias
{
    public static IServiceCollection AgregarNegocio(this IServiceCollection services, JwtOpciones jwt)
    {
        services.AddSingleton(jwt);
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IAuthServicio, AuthServicio>();
        services.AddScoped<IPlanServicio, PlanServicio>();
        services.AddScoped<IMiembroServicio, MiembroServicio>();
        return services;
    }
}
