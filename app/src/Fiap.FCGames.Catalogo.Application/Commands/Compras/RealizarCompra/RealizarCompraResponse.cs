namespace Fiap.FCGames.Catalogo.Application.Commands.Compras.RealizarCompra;

public record RealizarCompraResponse(Guid OrderId, Guid JogoId, string NomeJogo, decimal Preco, string Status);
