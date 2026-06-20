using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Compras.RealizarCompra;

public record RealizarCompraCommand(Guid UsuarioId, Guid JogoId) : IRequest<RealizarCompraResponse>;
