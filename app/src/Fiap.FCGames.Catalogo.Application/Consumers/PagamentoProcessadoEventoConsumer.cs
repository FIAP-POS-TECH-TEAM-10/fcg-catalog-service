using FCGames.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Fiap.FCGames.Catalogo.Application.Consumers;

public class PagamentoProcessadoEventoConsumer : IConsumer<PagamentoProcessadoEvento>
{
    private readonly ILogger<PagamentoProcessadoEventoConsumer> _logger;

    public PagamentoProcessadoEventoConsumer(ILogger<PagamentoProcessadoEventoConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<PagamentoProcessadoEvento> context)
    {
        // Implementado na Task 13 (FASE 4)
        _logger.LogInformation("PagamentoProcessadoEvento recebido para PedidoId {PedidoId} Status {Status}",
            context.Message.PedidoId, context.Message.Status);
        return Task.CompletedTask;
    }
}
