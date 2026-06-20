using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.Compras.BuscarPedido;

public class BuscarPedidoQueryHandler : IRequestHandler<BuscarPedidoQuery, BuscarPedidoResponse?>
{
    private readonly IUnitOfWork _uow;

    public BuscarPedidoQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<BuscarPedidoResponse?> Handle(BuscarPedidoQuery request, CancellationToken cancellationToken)
    {
        var pedido = await _uow.PedidoRepository.ObterPorIdAsync(request.PedidoId);
        if (pedido is null) return null;
        return new BuscarPedidoResponse(pedido.Id, pedido.UsuarioId, pedido.JogoId, pedido.Preco, pedido.Status.ToString(), pedido.CriadoEm);
    }
}
