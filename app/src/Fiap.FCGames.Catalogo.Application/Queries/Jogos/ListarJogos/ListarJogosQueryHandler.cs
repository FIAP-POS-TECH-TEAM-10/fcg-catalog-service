using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.Jogos.ListarJogos;

public class ListarJogosQueryHandler : IRequestHandler<ListarJogosQuery, List<JogoResponse>>
{
    private readonly IUnitOfWork _uow;

    public ListarJogosQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<List<JogoResponse>> Handle(ListarJogosQuery request, CancellationToken cancellationToken)
    {
        var jogos = await _uow.JogoRepository.ListarTodosAsync();
        return jogos.Select(j => new JogoResponse(j.Id, j.Nome, j.Descricao, j.Preco)).ToList();
    }
}
