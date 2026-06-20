using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateJogo;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;

public interface IJogoRepository
{
    Task<List<Jogo>> ListarTodosAsync();
    Task<Jogo?> ObterPorIdAsync(Guid id);
    void Adicionar(Jogo jogo);
    void Atualizar(Jogo jogo);
    void Remover(Jogo jogo);
}
