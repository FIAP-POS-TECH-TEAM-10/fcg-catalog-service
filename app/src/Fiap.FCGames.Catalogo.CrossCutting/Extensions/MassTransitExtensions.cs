using FCGames.IntegrationEvents;
using Fiap.FCGames.Catalogo.Application.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.FCGames.Catalogo.CrossCutting.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<UsuarioCriadoEventoConsumer>();
            x.AddConsumer<PagamentoProcessadoEventoConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                var host = configuration["RabbitMQ:Host"] ?? "localhost";
                var username = configuration["RabbitMQ:Username"] ?? "guest";
                var password = configuration["RabbitMQ:Password"] ?? "guest";

                cfg.Host(host, "/", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.ReceiveEndpoint("catalog-usuario-criado", e =>
                {
                    e.ConfigureConsumer<UsuarioCriadoEventoConsumer>(ctx);
                });

                cfg.ReceiveEndpoint("catalog-pagamento-processado", e =>
                {
                    e.ConfigureConsumer<PagamentoProcessadoEventoConsumer>(ctx);
                });
            });
        });

        return services;
    }
}
