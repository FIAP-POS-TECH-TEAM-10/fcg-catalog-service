using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregatePedido;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;

public interface IPedidoRepository
{
    Task<Pedido?> ObterPorIdAsync(Guid id);
    void Adicionar(Pedido pedido);
    void Atualizar(Pedido pedido);
}
