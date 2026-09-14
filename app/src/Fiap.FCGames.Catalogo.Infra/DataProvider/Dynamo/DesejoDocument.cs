using Amazon.DynamoDBv2.DataModel;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Dynamo;

[DynamoDBTable("Desejos")]
public class DesejoDocument
{
    [DynamoDBHashKey("UsuarioId")]
    public string UsuarioId { get; set; } = string.Empty;

    [DynamoDBRangeKey("JogoId")]
    public string JogoId { get; set; } = string.Empty;

    [DynamoDBProperty("AdicionadoEm")]
    public DateTime AdicionadoEm { get; set; }
}
