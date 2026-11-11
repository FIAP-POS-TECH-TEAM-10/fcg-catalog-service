using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.FCGames.Catalogo.CrossCutting.Extensions;

/// <summary>
/// Extensão de DI para registrar Elasticsearch/OpenSearch.
/// Em desenvolvimento: tenta conectar ao localhost:9200 (Docker)
/// Em produção: usa a URL do appsettings ou variável de ambiente
/// </summary>
public static class RegisterElasticsearchExtensions
{
    public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var elasticsearchUrl = configuration["Elasticsearch:Url"] ?? "http://localhost:9200";
        
        var client = new ElasticsearchClient(new Uri(elasticsearchUrl));
        
        services.AddSingleton(client);
    }
}
