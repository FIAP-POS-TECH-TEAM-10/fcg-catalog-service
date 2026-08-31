using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Dynamo;

public static class DynamoDbInitializer
{
    public static Task EnsureJogosTableExistsAsync(IAmazonDynamoDB client, CancellationToken cancellationToken = default)
        => EnsureTableExistsAsync(client, new CreateTableRequest
        {
            TableName = "Jogos",
            AttributeDefinitions = [new AttributeDefinition("Id", ScalarAttributeType.S)],
            KeySchema = [new KeySchemaElement("Id", KeyType.HASH)],
            BillingMode = BillingMode.PAY_PER_REQUEST
        }, cancellationToken);

    public static Task EnsureDesejosTableExistsAsync(IAmazonDynamoDB client, CancellationToken cancellationToken = default)
        => EnsureTableExistsAsync(client, new CreateTableRequest
        {
            TableName = "Desejos",
            AttributeDefinitions =
            [
                new AttributeDefinition("UsuarioId", ScalarAttributeType.S),
                new AttributeDefinition("JogoId", ScalarAttributeType.S)
            ],
            KeySchema =
            [
                new KeySchemaElement("UsuarioId", KeyType.HASH),
                new KeySchemaElement("JogoId", KeyType.RANGE)
            ],
            BillingMode = BillingMode.PAY_PER_REQUEST
        }, cancellationToken);

    private static async Task EnsureTableExistsAsync(IAmazonDynamoDB client, CreateTableRequest request, CancellationToken cancellationToken)
    {
        var tabelas = await client.ListTablesAsync(cancellationToken);
        if (tabelas.TableNames.Contains(request.TableName)) return;

        await client.CreateTableAsync(request, cancellationToken);

        while (true)
        {
            var status = await client.DescribeTableAsync(request.TableName, cancellationToken);
            if (status.Table.TableStatus == TableStatus.ACTIVE) return;
            await Task.Delay(500, cancellationToken);
        }
    }
}
