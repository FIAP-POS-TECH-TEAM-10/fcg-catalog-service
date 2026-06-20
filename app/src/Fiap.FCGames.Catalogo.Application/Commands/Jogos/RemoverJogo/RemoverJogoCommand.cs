using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Jogos.RemoverJogo;

public record RemoverJogoCommand(Guid Id) : IRequest<Unit>;
