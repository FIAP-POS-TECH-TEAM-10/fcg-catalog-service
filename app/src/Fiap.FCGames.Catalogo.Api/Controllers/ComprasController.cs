using Fiap.FCGames.Catalogo.Api.Controllers.Shared;
using Fiap.FCGames.Catalogo.Application.Commands.Compras.RealizarCompra;
using Fiap.FCGames.Catalogo.Application.Queries.Compras.BuscarPedido;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fiap.FCGames.Catalogo.Api.Controllers;

[ApiController]
[Route("compras")]
public class ComprasController : ApiControllerBase<ComprasController>
{
    public ComprasController(ISender sender, ILogger<ComprasController> logger) : base(sender, logger) { }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RealizarAsync([FromBody] RealizarCompraRequest request, CancellationToken cancellationToken)
    {
        var usuarioId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _sender.Send(new RealizarCompraCommand(usuarioId, request.JogoId), cancellationToken);
        return Accepted(new { orderId = result.OrderId, jogoId = result.JogoId, nomeJogo = result.NomeJogo, preco = result.Preco, status = result.Status });
    }

    [HttpGet("{orderId:guid}")]
    [Authorize]
    public async Task<IActionResult> BuscarAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new BuscarPedidoQuery(orderId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}

public record RealizarCompraRequest(Guid JogoId);
