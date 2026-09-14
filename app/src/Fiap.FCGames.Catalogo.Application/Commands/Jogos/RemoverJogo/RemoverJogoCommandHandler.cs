using Fiap.FCGames.Catalogo.Domain.Exception;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Fiap.FCGames.Catalogo.Application.Commands.Jogos.RemoverJogo;

public class RemoverJogoCommandHandler : IRequestHandler<RemoverJogoCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IDistributedCache _cache;

    public RemoverJogoCommandHandler(IUnitOfWork uow, IDistributedCache cache)
    {
        _uow = uow;
        _cache = cache;
    }

    public async Task<Unit> Handle(RemoverJogoCommand request, CancellationToken cancellationToken)
    {
        var jogo = await _uow.JogoRepository.ObterPorIdAsync(request.Id)
            ?? throw new NotFoundException($"Jogo {request.Id} não encontrado.");

        await _uow.JogoRepository.RemoverAsync(jogo);
        await _cache.RemoveAsync("catalogo:jogos", cancellationToken);

        return Unit.Value;
    }
}
