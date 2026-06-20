using FCGames.IntegrationEvents;
using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateBiblioteca;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Fiap.FCGames.Catalogo.Application.Consumers;

public class UsuarioCriadoEventoConsumer : IConsumer<UsuarioCriadoEvento>
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<UsuarioCriadoEventoConsumer> _logger;

    public UsuarioCriadoEventoConsumer(IUnitOfWork uow, ILogger<UsuarioCriadoEventoConsumer> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UsuarioCriadoEvento> context)
    {
        var evt = context.Message;

        var existe = await _uow.BibliotecaRepository.ObterPorUsuarioIdAsync(evt.UsuarioId);
        if (existe is not null)
        {
            _logger.LogInformation("Biblioteca já existe para UsuarioId {UsuarioId} — ignorando (idempotente)", evt.UsuarioId);
            return;
        }

        var biblioteca = new Biblioteca
        {
            Id = Guid.NewGuid(),
            UsuarioId = evt.UsuarioId,
            CriadaEm = DateTime.UtcNow
        };

        _uow.BibliotecaRepository.Adicionar(biblioteca);
        await _uow.CommitAsync(context.CancellationToken);

        _logger.LogInformation("Biblioteca criada para UsuarioId {UsuarioId} CorrelationId {CorrelationId}",
            evt.UsuarioId, evt.CorrelationId);
    }
}
