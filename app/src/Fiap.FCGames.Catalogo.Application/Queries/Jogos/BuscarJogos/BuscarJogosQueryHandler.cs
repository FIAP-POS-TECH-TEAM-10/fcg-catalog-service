using Fiap.FCGames.Catalogo.Application.Services;
using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.Jogos.BuscarJogos;

/// <summary>
/// Handler da query BuscarJogos.
/// Delega a busca para o ElasticsearchService que retorna resultados ordenados por relevância.
/// </summary>
public class BuscarJogosQueryHandler : IRequestHandler<BuscarJogosQuery, List<BuscarJogosResponse>>
{
    private readonly ElasticsearchService _elasticsearchService;

    public BuscarJogosQueryHandler(ElasticsearchService elasticsearchService)
    {
        _elasticsearchService = elasticsearchService;
    }

    public async Task<List<BuscarJogosResponse>> Handle(BuscarJogosQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Termo))
            return new List<BuscarJogosResponse>();

        var resultados = await _elasticsearchService.BuscarJogosAsync(
            request.Termo.Trim(),
            request.Fuzzy,
            request.Tamanho,
            cancellationToken);

        return resultados
            .Select(r => new BuscarJogosResponse(r.Id, r.Nome, r.Descricao, r.Preco))
            .ToList();
    }
}
