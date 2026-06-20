namespace Fiap.FCGames.Catalogo.Application.Commands.Jogos.CriarJogo;

public record CriarJogoResponse(Guid Id, string Nome, string Descricao, decimal Preco, DateTime DataCadastro);
