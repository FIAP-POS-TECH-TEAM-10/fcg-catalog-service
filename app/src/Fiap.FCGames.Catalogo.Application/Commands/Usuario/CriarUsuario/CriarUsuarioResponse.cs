namespace Fiap.FCGames.Catalogo.Application.Commands.Usuario.CriarUsuario;

public class CriarUsuarioResponse
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public Guid BibliotecaId { get; set; } = Guid.Empty;
}
