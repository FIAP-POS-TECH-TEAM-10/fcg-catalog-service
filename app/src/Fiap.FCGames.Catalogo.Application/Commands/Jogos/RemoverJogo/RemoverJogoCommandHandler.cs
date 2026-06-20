using Fiap.FCGames.Catalogo.Domain.Exception;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Jogos.RemoverJogo;

public class RemoverJogoCommandHandler : IRequestHandler<RemoverJogoCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public RemoverJogoCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(RemoverJogoCommand request, CancellationToken cancellationToken)
    {
        var jogo = await _uow.JogoRepository.ObterPorIdAsync(request.Id)
            ?? throw new NotFoundException($"Jogo {request.Id} não encontrado.");

        _uow.JogoRepository.Remover(jogo);
        await _uow.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
