using Amazon.DynamoDBv2.DataModel;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Dynamo;

[DynamoDBTable("Jogos")]
public class JogoDocument
{
    [DynamoDBHashKey("Id")]
    public string Id { get; set; } = string.Empty;

    [DynamoDBProperty("Nome")]
    public string Nome { get; set; } = string.Empty;

    [DynamoDBProperty("Descricao")]
    public string Descricao { get; set; } = string.Empty;

    [DynamoDBProperty("Preco")]
    public decimal Preco { get; set; }

    [DynamoDBProperty("DataCadastro")]
    public DateTime DataCadastro { get; set; }
}
