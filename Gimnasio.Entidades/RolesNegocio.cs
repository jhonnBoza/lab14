namespace Gimnasio.Entidades;

public static class RolesNegocio
{
    public const string Recepcionista = "Recepcionista";
    public const string Entrenador = "Entrenador";

    // Quien se registra solo no elige rol: queda como entrenador.
    public const string PorDefecto = Entrenador;
}
