using Fiap.FCGames.Catalogo.Api.Controllers.Shared;
using Fiap.FCGames.Catalogo.Application.Queries.BibliotecaJogos.BuscarBibliotecaJogos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.FCGames.Catalogo.Api.Controllers;

[ApiController]
[Route("biblioteca-jogos")]
public class BibliotecaJogosController : ApiControllerBase<BibliotecaJogosController>
{
    public BibliotecaJogosController(ISender sender, ILogger<BibliotecaJogosController> logger) : base(sender, logger) { }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> BuscarAsync([FromQuery] Guid usuario_id)
    {
        var result = await _sender.Send(new BuscarBibliotecaJogosQuery(usuario_id));

        if (result is not null)
            return Ok(result);

        return NoContent();
    }
}
