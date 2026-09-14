using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateDesejo;
using Fiap.FCGames.Catalogo.Domain.Exception;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Desejos.AdicionarDesejo;

public class AdicionarDesejoCommandHandler : IRequestHandler<AdicionarDesejoCommand, AdicionarDesejoResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly IDesejoRepository _desejoRepository;

    public AdicionarDesejoCommandHandler(IUnitOfWork uow, IDesejoRepository desejoRepository)
    {
        _uow = uow;
        _desejoRepository = desejoRepository;
    }

    public async Task<AdicionarDesejoResponse> Handle(AdicionarDesejoCommand request, CancellationToken cancellationToken)
    {
        var jogo = await _uow.JogoRepository.ObterPorIdAsync(request.JogoId)
            ?? throw new NotFoundException($"Jogo '{request.JogoId}' não encontrado.");

        var desejo = new Desejo
        {
            UsuarioId = request.UsuarioId,
            JogoId = request.JogoId,
            AdicionadoEm = DateTime.UtcNow
        };

        await _desejoRepository.AdicionarAsync(desejo);

        return new AdicionarDesejoResponse(jogo.Id, jogo.Nome, desejo.AdicionadoEm);
    }
}
