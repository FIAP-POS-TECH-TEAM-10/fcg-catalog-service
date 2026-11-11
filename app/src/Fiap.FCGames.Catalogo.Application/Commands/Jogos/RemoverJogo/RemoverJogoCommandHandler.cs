using Fiap.FCGames.Catalogo.Application.Services;
using Fiap.FCGames.Catalogo.Domain.Exception;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Fiap.FCGames.Catalogo.Application.Commands.Jogos.RemoverJogo;

public class RemoverJogoCommandHandler : IRequestHandler<RemoverJogoCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IDistributedCache _cache;
    private readonly ElasticsearchService _elasticsearchService;

    public RemoverJogoCommandHandler(IUnitOfWork uow, IDistributedCache cache, ElasticsearchService elasticsearchService)
    {
        _uow = uow;
        _cache = cache;
        _elasticsearchService = elasticsearchService;
    }

    public async Task<Unit> Handle(RemoverJogoCommand request, CancellationToken cancellationToken)
    {
        var jogo = await _uow.JogoRepository.ObterPorIdAsync(request.Id)
            ?? throw new NotFoundException($"Jogo {request.Id} não encontrado.");

        await _uow.JogoRepository.RemoverAsync(jogo);
        await _cache.RemoveAsync("catalogo:jogos", cancellationToken);
        
        // Desindexar do Elasticsearch de forma assíncrona (fire-and-forget)
        _ = _elasticsearchService.RemoverJogoAsync(request.Id, cancellationToken);

        return Unit.Value;
    }
}
