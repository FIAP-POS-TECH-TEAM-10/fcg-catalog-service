using Fiap.FCGames.Catalogo.Application.Services;
using Fiap.FCGames.Catalogo.Domain.Exception;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Fiap.FCGames.Catalogo.Application.Commands.Jogos.AtualizarJogo;

public class AtualizarJogoCommandHandler : IRequestHandler<AtualizarJogoCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IDistributedCache _cache;
    private readonly ElasticsearchService _elasticsearchService;

    public AtualizarJogoCommandHandler(IUnitOfWork uow, IDistributedCache cache, ElasticsearchService elasticsearchService)
    {
        _uow = uow;
        _cache = cache;
        _elasticsearchService = elasticsearchService;
    }

    public async Task<Unit> Handle(AtualizarJogoCommand request, CancellationToken cancellationToken)
    {
        var jogo = await _uow.JogoRepository.ObterPorIdAsync(request.Id)
            ?? throw new NotFoundException($"Jogo {request.Id} não encontrado.");

        jogo.Nome = request.Nome;
        jogo.Descricao = request.Descricao;
        jogo.Preco = request.Preco;

        await _uow.JogoRepository.AtualizarAsync(jogo);
        await _cache.RemoveAsync("catalogo:jogos", cancellationToken);
        
        // Sincronizar índice Elasticsearch de forma assíncrona (fire-and-forget)
        _ = _elasticsearchService.IndexarJogoAsync(jogo.Id, jogo.Nome, jogo.Descricao, jogo.Preco, cancellationToken);

        return Unit.Value;
    }
}
