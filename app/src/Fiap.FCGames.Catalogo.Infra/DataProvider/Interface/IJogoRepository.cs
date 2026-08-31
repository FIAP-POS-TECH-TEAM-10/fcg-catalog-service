using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateJogo;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;

public interface IJogoRepository
{
    Task<List<Jogo>> ListarTodosAsync();
    Task<Jogo?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Jogo jogo);
    Task AtualizarAsync(Jogo jogo);
    Task RemoverAsync(Jogo jogo);
}
