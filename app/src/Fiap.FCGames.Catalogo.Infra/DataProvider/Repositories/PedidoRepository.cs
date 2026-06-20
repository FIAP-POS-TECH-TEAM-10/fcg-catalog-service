using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregatePedido;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Contexto;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Repositories;

public class PedidoRepository : GenericRepository<Pedido>, IPedidoRepository
{
    public PedidoRepository(FcGamesContexto context) : base(context) { }

    public async Task<Pedido?> ObterPorIdAsync(Guid id)
        => await Get(p => p.Id == id).AsNoTracking().FirstOrDefaultAsync();

    public void Adicionar(Pedido pedido) => Create(pedido);

    public void Atualizar(Pedido pedido) => Update(pedido);
}
