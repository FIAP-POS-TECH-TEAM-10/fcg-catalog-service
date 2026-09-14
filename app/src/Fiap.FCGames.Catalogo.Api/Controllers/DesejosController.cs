using Fiap.FCGames.Catalogo.Api.Controllers.Shared;
using Fiap.FCGames.Catalogo.Application.Commands.Desejos.AdicionarDesejo;
using Fiap.FCGames.Catalogo.Application.Commands.Desejos.RemoverDesejo;
using Fiap.FCGames.Catalogo.Application.Queries.Desejos.ListarDesejos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fiap.FCGames.Catalogo.Api.Controllers;

[ApiController]
[Route("desejos")]
public class DesejosController : ApiControllerBase<DesejosController>
{
    public DesejosController(ISender sender, ILogger<DesejosController> logger) : base(sender, logger) { }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ListarAsync(CancellationToken cancellationToken)
    {
        var usuarioId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _sender.Send(new ListarDesejosQuery(usuarioId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AdicionarAsync([FromBody] AdicionarDesejoRequest request, CancellationToken cancellationToken)
    {
        var usuarioId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _sender.Send(new AdicionarDesejoCommand(usuarioId, request.JogoId), cancellationToken);
        return Created($"/desejos/{result.JogoId}", result);
    }

    [HttpDelete("{jogoId:guid}")]
    [Authorize]
    public async Task<IActionResult> RemoverAsync(Guid jogoId, CancellationToken cancellationToken)
    {
        var usuarioId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _sender.Send(new RemoverDesejoCommand(usuarioId, jogoId), cancellationToken);
        return Ok();
    }
}

public record AdicionarDesejoRequest(Guid JogoId);
