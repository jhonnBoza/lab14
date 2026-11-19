namespace Gimnasio.Entidades.Dtos;

public class TokenDto
{
    public string Token { get; set; } = null!;

    public DateTime Expiracion { get; set; }
}
