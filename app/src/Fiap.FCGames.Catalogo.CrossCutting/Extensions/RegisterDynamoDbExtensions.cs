using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.FCGames.Catalogo.CrossCutting.Extensions;

public static class RegisterDynamoDbExtensions
{
    public static void AddDynamoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAmazonDynamoDB>(_ =>
        {
            var region = configuration["DynamoDb:Region"] ?? "us-east-1";
            var config = new AmazonDynamoDBConfig { RegionEndpoint = RegionEndpoint.GetBySystemName(region) };

            // ServiceUrl aponta pro DynamoDB Local em dev, em produção (AWS real) fica vazio.
            var serviceUrl = configuration["DynamoDb:ServiceUrl"];
            if (!string.IsNullOrWhiteSpace(serviceUrl))
                config.ServiceURL = serviceUrl;

            return new AmazonDynamoDBClient(config);
        });

        services.AddSingleton<IDynamoDBContext>(sp => new DynamoDBContext(sp.GetRequiredService<IAmazonDynamoDB>()));
    }
}
