using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Jogos.AtualizarJogo;

public record AtualizarJogoCommand(Guid Id, string Nome, string Descricao, decimal Preco) : IRequest<Unit>;
