using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateJogo;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Contexto;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories;

public class JogoRepository : GenericRepository<Jogo>, IJogoRepository
{
    public JogoRepository(FcGamesContexto context) : base(context) { }

    public async Task<List<Jogo>> ListarTodosAsync()
        => await GetAll().AsNoTracking().ToListAsync();

    public async Task<Jogo?> ObterPorIdAsync(Guid id)
        => await Get(j => j.Id == id).AsNoTracking().FirstOrDefaultAsync();

    public void Adicionar(Jogo jogo) => Create(jogo);

    public void Atualizar(Jogo jogo) => Update(jogo);

    public void Remover(Jogo jogo) => Delete(j => j.Id == jogo.Id);
}
