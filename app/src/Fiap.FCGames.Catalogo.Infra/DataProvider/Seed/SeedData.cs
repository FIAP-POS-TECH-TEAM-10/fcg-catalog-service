using Amazon.DynamoDBv2.DataModel;
using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateJogo;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Dynamo;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Seed;

public static class SeedData
{
    public static async Task SeedJogosAsync(IDynamoDBContext dynamoContext)
    {
        var existentes = await dynamoContext.ScanAsync<JogoDocument>([]).GetRemainingAsync();
        if (existentes.Count > 0) return;

        var jogos = new List<Jogo>
        {
            new() { Id = Guid.NewGuid(), Nome = "Hades", Descricao = "Roguelike aclamado pela crítica", Preco = 49.90m, DataCadastro = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Nome = "Hollow Knight", Descricao = "Metroidvania desafiador e atmosférico", Preco = 29.90m, DataCadastro = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Nome = "Cyberpunk 2077", Descricao = "RPG de ação em mundo aberto futurista", Preco = 149.90m, DataCadastro = DateTime.UtcNow },
        };

        foreach (var jogo in jogos)
        {
            await dynamoContext.SaveAsync(new JogoDocument
            {
                Id = jogo.Id.ToString(),
                Nome = jogo.Nome,
                Descricao = jogo.Descricao,
                Preco = jogo.Preco,
                DataCadastro = jogo.DataCadastro
            });
        }
    }
}
