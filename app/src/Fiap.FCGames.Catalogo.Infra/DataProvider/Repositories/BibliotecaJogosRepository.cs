using Fiap.FCGames.Catalogo.Domain.Aggregates;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Contexto;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories;

public class BibliotecaJogosRepository : GenericRepository<BibliotecaJogos>, IBibliotecaJogosRepository
{
    public BibliotecaJogosRepository(FcGamesContexto context) : base(context) { }

    public void Adicionar(BibliotecaJogos biblioteca) => Create(biblioteca);

    public async Task<BibliotecaJogos?> ObterPorUsuarioIdAsync(UsuarioId usuarioId)
        => await Get(b => b.IdUsuario == usuarioId).AsNoTracking().FirstOrDefaultAsync();
}
