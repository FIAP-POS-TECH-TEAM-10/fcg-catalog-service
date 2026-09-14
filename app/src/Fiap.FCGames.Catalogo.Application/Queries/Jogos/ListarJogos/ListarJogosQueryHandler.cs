using System.Text.Json;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Fiap.FCGames.Catalogo.Application.Queries.Jogos.ListarJogos;

public class ListarJogosQueryHandler : IRequestHandler<ListarJogosQuery, List<JogoResponse>>
{
    private const string CacheKey = "catalogo:jogos";

    private readonly IUnitOfWork _uow;
    private readonly IDistributedCache _cache;
    private readonly ILogger<ListarJogosQueryHandler> _logger;

    public ListarJogosQueryHandler(IUnitOfWork uow, IDistributedCache cache, ILogger<ListarJogosQueryHandler> logger)
    {
        _uow = uow;
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<JogoResponse>> Handle(ListarJogosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var cached = await _cache.GetStringAsync(CacheKey, cancellationToken);
            if (cached is not null)
            {
                var jogosCache = JsonSerializer.Deserialize<List<JogoResponse>>(cached);
                if (jogosCache is not null)
                    return jogosCache;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao ler o cache de jogos, seguindo para o DynamoDB.");
        }

        var jogos = await _uow.JogoRepository.ListarTodosAsync();
        var response = jogos.Select(j => new JogoResponse(j.Id, j.Nome, j.Descricao, j.Preco)).ToList();

        try
        {
            await _cache.SetStringAsync(
                CacheKey,
                JsonSerializer.Serialize(response),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                },
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao gravar o cache de jogos.");
        }

        return response;
    }
}
