using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.Desejos.ListarDesejos;

public class ListarDesejosQueryHandler : IRequestHandler<ListarDesejosQuery, List<DesejoResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IDesejoRepository _desejoRepository;

    public ListarDesejosQueryHandler(IUnitOfWork uow, IDesejoRepository desejoRepository)
    {
        _uow = uow;
        _desejoRepository = desejoRepository;
    }

    public async Task<List<DesejoResponse>> Handle(ListarDesejosQuery request, CancellationToken cancellationToken)
    {
        var desejos = await _desejoRepository.ListarPorUsuarioAsync(request.UsuarioId);
        if (desejos.Count == 0) return [];

        var jogoIds = desejos.Select(d => d.JogoId).ToHashSet();
        var todosJogos = await _uow.JogoRepository.ListarTodosAsync();
        var jogosMap = todosJogos.Where(j => jogoIds.Contains(j.Id)).ToDictionary(j => j.Id);

        return desejos.Select(d =>
        {
            jogosMap.TryGetValue(d.JogoId, out var jogo);
            return new DesejoResponse(d.JogoId, jogo?.Nome ?? "Desconhecido", jogo?.Preco ?? 0, d.AdicionadoEm);
        }).ToList();
    }
}
