using FCGames.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Fiap.FCGames.Catalogo.Application.Consumers;

public class UsuarioCriadoEventoConsumer : IConsumer<UsuarioCriadoEvento>
{
    private readonly ILogger<UsuarioCriadoEventoConsumer> _logger;

    public UsuarioCriadoEventoConsumer(ILogger<UsuarioCriadoEventoConsumer> logger) => _logger = logger;

    public Task Consume(ConsumeContext<UsuarioCriadoEvento> context)
    {
        // Implementado na Task 12 (FASE 4)
        _logger.LogInformation("UsuarioCriadoEvento recebido para {UsuarioId}", context.Message.UsuarioId);
        return Task.CompletedTask;
    }
}
