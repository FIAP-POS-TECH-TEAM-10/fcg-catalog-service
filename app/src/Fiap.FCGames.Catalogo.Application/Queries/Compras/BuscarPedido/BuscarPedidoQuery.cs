using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.Compras.BuscarPedido;

public record BuscarPedidoQuery(Guid PedidoId) : IRequest<BuscarPedidoResponse?>;
