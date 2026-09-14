using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateJogo;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Fiap.FCGames.Catalogo.Application.Commands.Jogos.CriarJogo;

public class CriarJogoCommandHandler : IRequestHandler<CriarJogoCommand, CriarJogoResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly IDistributedCache _cache;

    public CriarJogoCommandHandler(IUnitOfWork uow, IDistributedCache cache)
    {
        _uow = uow;
        _cache = cache;
    }

    public async Task<CriarJogoResponse> Handle(CriarJogoCommand request, CancellationToken cancellationToken)
    {
        var jogo = new Jogo
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            Descricao = request.Descricao,
            Preco = request.Preco,
            DataCadastro = DateTime.UtcNow
        };

        await _uow.JogoRepository.AdicionarAsync(jogo);
        await _cache.RemoveAsync("catalogo:jogos", cancellationToken);

        return new CriarJogoResponse(jogo.Id, jogo.Nome, jogo.Descricao, jogo.Preco, jogo.DataCadastro);
    }
}
