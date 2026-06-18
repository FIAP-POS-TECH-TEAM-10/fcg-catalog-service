using Fiap.FCGames.Catalogo.Domain.Aggregates;

namespace Fiap.FCGames.Catalogo.Application.Queries.Usuario.BuscarUsuarioPorId;

public class DetalhesUsuarioDto
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string NomeUsuario { get; set; }
    public DateTime? DataCadastro { get; set; }
    public TipoAcesso TipoAcesso { get; set; }
}
