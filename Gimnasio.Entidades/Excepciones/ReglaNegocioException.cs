namespace Gimnasio.Entidades.Excepciones;

public class ReglaNegocioException : Exception
{
    public int CodigoHttp { get; }

    public ReglaNegocioException(string mensaje, int codigoHttp = 409) : base(mensaje)
    {
        CodigoHttp = codigoHttp;
    }
}
