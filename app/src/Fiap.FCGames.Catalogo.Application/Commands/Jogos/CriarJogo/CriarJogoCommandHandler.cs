using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateJogo;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Jogos.CriarJogo;

public class CriarJogoCommandHandler : IRequestHandler<CriarJogoCommand, CriarJogoResponse>
{
    private readonly IUnitOfWork _uow;

    public CriarJogoCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CriarJogoResponse> Handle(CriarJogoCommand request, CancellationToken cancellationToken)
    {
        var jogo = new Jogo
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            Descricao = request.Descricao,
            Preco = request.Preco,
            DataCadastro = DateTime.UtcNow
        };

        _uow.JogoRepository.Adicionar(jogo);
        await _uow.CommitAsync(cancellationToken);

        return new CriarJogoResponse(jogo.Id, jogo.Nome, jogo.Descricao, jogo.Preco, jogo.DataCadastro);
    }
}
