using Gimnasio.Entidades.Excepciones;
using Microsoft.AspNetCore.Diagnostics;

namespace Gimnasio.Api.Filtros;

public class ReglaNegocioHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ReglaNegocioException regla)
        {
            return false;
        }

        httpContext.Response.StatusCode = regla.CodigoHttp;
        await httpContext.Response.WriteAsJsonAsync(new { mensaje = regla.Message }, cancellationToken);
        return true;
    }
}
