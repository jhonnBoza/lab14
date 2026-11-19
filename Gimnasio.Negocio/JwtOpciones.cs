namespace Gimnasio.Negocio;

public class JwtOpciones
{
    public string Key { get; set; } = "";

    public string Issuer { get; set; } = "";

    public string Audience { get; set; } = "";

    public int ExpiracionMinutos { get; set; } = 60;
}
