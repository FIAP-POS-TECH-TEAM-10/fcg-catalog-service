using Fiap.FCGames.Catalogo.Api.Controllers.Shared;
using Fiap.FCGames.Catalogo.Application.Commands.Usuario.AtualizarUsuario;
using Fiap.FCGames.Catalogo.Application.Commands.Usuario.CriarUsuario;
using Fiap.FCGames.Catalogo.Application.Queries.Usuario.BuscarUsuarioPorId;
using Fiap.FCGames.Catalogo.Application.Queries.Usuario.ListarUsuarios;
using Fiap.FCGames.Catalogo.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.FCGames.Catalogo.Api.Controllers;

[ApiController]
[Route("usuarios")]
public class UsuarioController : ApiControllerBase<UsuarioController>
{
    public UsuarioController(ISender sender, ILogger<UsuarioController> logger)
        : base(sender, logger) { }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CriarAsync([FromBody] CriarUsuarioCommand command)
    {
        var result = await _sender.Send(command);
        return StatusCode(201, result);
    }

    [HttpPut]
    [Authorize(Policy = AuthConstants.AdminPolicy)]
    public async Task<IActionResult> AtualizarAsync([FromBody] AtualizarUsuarioCommand command)
    {
        var result = await _sender.Send(command);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = AuthConstants.AdminPolicy)]
    public async Task<IActionResult> ListarTodos()
    {
        var result = await _sender.Send(new ListarUsuariosQuery());

        if (result == null)
            return NoContent();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = AuthConstants.AdminPolicy)]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var result = await _sender.Send(new BuscarUsuarioPorIdQuery(id));

        if (result == null)
            return NotFound();
        return Ok(result);
    }
}
