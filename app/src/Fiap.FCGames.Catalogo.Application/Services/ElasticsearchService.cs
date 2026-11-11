using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;

namespace Fiap.FCGames.Catalogo.Application.Services;

/// <summary>
/// Serviço de integração com Elasticsearch.
/// Responsável por indexar, atualizar, remover documentos de jogos e realizar buscas.
/// </summary>
public class ElasticsearchService
{
    private const string IndexName = "jogos";
    private readonly ElasticsearchClient _client;
    private readonly ILogger<ElasticsearchService> _logger;

    public ElasticsearchService(ElasticsearchClient client, ILogger<ElasticsearchService> logger)
    {
        _client = client;
        _logger = logger;
    }

    /// <summary>
    /// Cria o índice de jogos se não existir.
    /// Chamado no startup da aplicação.
    /// </summary>
    public async Task EnsureIndexExistsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var existsResponse = await _client.Indices.ExistsAsync(IndexName, cancellationToken: cancellationToken);
            
            if (existsResponse.Exists)
            {
                _logger.LogInformation("Índice '{IndexName}' já existe", IndexName);
                return;
            }

            var createIndexResponse = await _client.Indices.CreateAsync(IndexName, c => c
                .Settings(s => s.NumberOfShards(1).NumberOfReplicas(0))
            , cancellationToken: cancellationToken);

            if (createIndexResponse.Acknowledged)
            {
                _logger.LogInformation("Índice '{IndexName}' criado com sucesso", IndexName);
            }
            else
            {
                _logger.LogWarning("Falha ao criar índice '{IndexName}'", IndexName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao garantir que o índice '{IndexName}' existe", IndexName);
        }
    }

    /// <summary>
    /// Indexa um jogo no Elasticsearch (INSERT ou UPDATE).
    /// Chamado após CriarJogo ou AtualizarJogo (fire-and-forget).
    /// </summary>
    public async Task IndexarJogoAsync(Guid id, string nome, string descricao, decimal preco, CancellationToken cancellationToken = default)
    {
        try
        {
            var documento = new JogoDocument
            {
                Id = id.ToString(),
                Nome = nome.ToLower(),
                Descricao = descricao.ToLower(),
                Preco = preco,
                IndexadoEm = DateTime.UtcNow
            };

            var response = await _client.IndexAsync(
                documento,
                i => i.Index(IndexName).Id(id.ToString()),
                cancellationToken: cancellationToken);

            if (response.IsValidResponse)
            {
                _logger.LogInformation("Jogo {JogoId} indexado com sucesso", id);
            }
            else
            {
                _logger.LogWarning("Falha ao indexar jogo {JogoId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao indexar jogo {JogoId}", id);
        }
    }

    /// <summary>
    /// Remove um jogo do índice Elasticsearch.
    /// Chamado após RemoverJogo.
    /// </summary>
    public async Task RemoverJogoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.DeleteAsync<JogoDocument>(
                IndexName,
                id.ToString(),
                cancellationToken: cancellationToken);

            if (response.IsValidResponse)
            {
                _logger.LogInformation("Jogo {JogoId} removido do índice com sucesso", id);
            }
            else
            {
                _logger.LogWarning("Falha ao remover jogo {JogoId} do índice", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover jogo {JogoId} do índice", id);
        }
    }

    /// <summary>
    /// Busca jogos por termo com suporte a fuzzy search e ordenação por relevância.
    /// </summary>
    public async Task<List<BuscaJogosResponse>> BuscarJogosAsync(
        string termo,
        bool fuzzySearch = true,
        int tamanho = 20,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(termo))
            {
                return new List<BuscaJogosResponse>();
            }

            var searchResponse = await _client.SearchAsync<JogoDocument>(s => s
                .Index(IndexName)
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Query(termo)
                        .Fields(new[] { "nome^2", "descricao" })
                        .Fuzziness(fuzzySearch ? new Fuzziness("AUTO") : new Fuzziness(0))
                    )
                )
                .Size(tamanho)
                .TrackScores(true)
                .Sort(so => so.Score(new ScoreSort { Order = SortOrder.Desc })),
                cancellationToken: cancellationToken);

            if (!searchResponse.IsValidResponse)
            {
                _logger.LogWarning("Busca no Elasticsearch falhou");
                return new List<BuscaJogosResponse>();
            }

            var resultados = new List<BuscaJogosResponse>();
            foreach (var hit in searchResponse.Hits)
            {
                if (hit.Source != null)
                {
                    resultados.Add(new BuscaJogosResponse(
                        Guid.Parse(hit.Source.Id),
                        hit.Source.Nome,
                        hit.Source.Descricao,
                        hit.Source.Preco
                    ));
                }
            }

            _logger.LogInformation("Busca por '{Termo}' retornou {Count} resultados", termo, resultados.Count);
            return resultados;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar jogos por termo '{Termo}'", termo);
            return new List<BuscaJogosResponse>();
        }
    }

    /// <summary>
    /// Reindexar todos os documentos (refresh do índice).
    /// </summary>
    public async Task RefreshIndexAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.Indices.RefreshAsync(IndexName, cancellationToken: cancellationToken);
            if (response.IsValidResponse)
            {
                _logger.LogInformation("Índice '{IndexName}' recarregado com sucesso", IndexName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao recarregar índice '{IndexName}'", IndexName);
        }
    }
}

/// <summary>
/// Documento de jogo armazenado no Elasticsearch.
/// </summary>
public class JogoDocument
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public DateTime IndexadoEm { get; set; }
}

/// <summary>
/// Response DTO para resultados de busca.
/// </summary>
public record BuscaJogosResponse(Guid Id, string Nome, string Descricao, decimal Preco);
