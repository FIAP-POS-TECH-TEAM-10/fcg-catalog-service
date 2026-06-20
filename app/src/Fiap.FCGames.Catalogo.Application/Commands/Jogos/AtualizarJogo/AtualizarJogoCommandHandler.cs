using Fiap.FCGames.Catalogo.Domain.Exception;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Jogos.AtualizarJogo;

public class AtualizarJogoCommandHandler : IRequestHandler<AtualizarJogoCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public AtualizarJogoCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(AtualizarJogoCommand request, CancellationToken cancellationToken)
    {
        var jogo = await _uow.JogoRepository.ObterPorIdAsync(request.Id)
            ?? throw new NotFoundException($"Jogo {request.Id} não encontrado.");

        jogo.Nome = request.Nome;
        jogo.Descricao = request.Descricao;
        jogo.Preco = request.Preco;

        _uow.JogoRepository.Atualizar(jogo);
        await _uow.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
