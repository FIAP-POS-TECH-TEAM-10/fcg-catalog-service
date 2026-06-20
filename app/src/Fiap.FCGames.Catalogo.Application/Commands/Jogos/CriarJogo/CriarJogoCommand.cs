using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Jogos.CriarJogo;

public record CriarJogoCommand(string Nome, string Descricao, decimal Preco) : IRequest<CriarJogoResponse>;
