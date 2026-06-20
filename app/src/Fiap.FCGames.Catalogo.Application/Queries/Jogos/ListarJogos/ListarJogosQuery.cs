using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.Jogos.ListarJogos;

public record ListarJogosQuery : IRequest<List<JogoResponse>>;
