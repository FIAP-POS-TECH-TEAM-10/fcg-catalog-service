using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateBiblioteca;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Contexto;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories;

public class BibliotecaRepository : GenericRepository<Biblioteca>, IBibliotecaRepository
{
    private readonly FcGamesContexto _context;

    public BibliotecaRepository(FcGamesContexto context) : base(context)
    {
        _context = context;
    }

    public async Task<Biblioteca?> ObterPorUsuarioIdAsync(Guid usuarioId)
        => await Get(b => b.UsuarioId == usuarioId).AsNoTracking().FirstOrDefaultAsync();

    public async Task<Biblioteca?> ObterComItensPorUsuarioIdAsync(Guid usuarioId)
        => await _context.Bibliotecas
            .Include(b => b.Itens)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.UsuarioId == usuarioId);

    public async Task<bool> UsuarioPossuiJogoAsync(Guid usuarioId, Guid jogoId)
    {
        var biblioteca = await ObterPorUsuarioIdAsync(usuarioId);
        if (biblioteca is null) return false;

        return await _context.ItensBiblioteca
            .AsNoTracking()
            .AnyAsync(i => i.BibliotecaId == biblioteca.Id && i.JogoId == jogoId);
    }

    public void Adicionar(Biblioteca biblioteca) => Create(biblioteca);

    public void AdicionarItem(ItemBiblioteca item) => _context.ItensBiblioteca.Add(item);
}
