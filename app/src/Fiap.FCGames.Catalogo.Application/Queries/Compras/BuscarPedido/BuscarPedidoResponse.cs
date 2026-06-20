namespace Fiap.FCGames.Catalogo.Application.Queries.Compras.BuscarPedido;

public record BuscarPedidoResponse(Guid Id, Guid UsuarioId, Guid JogoId, decimal Preco, string Status, DateTime CriadoEm);
