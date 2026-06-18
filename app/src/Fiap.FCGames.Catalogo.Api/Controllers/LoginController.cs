using Fiap.FCGames.Catalogo.Api.Controllers.Shared;
using Fiap.FCGames.Catalogo.Application.Commands.Login;
using Fiap.FCGames.Catalogo.Domain.Exception;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.FCGames.Catalogo.Api.Controllers;

[ApiController]
[Route("login")]
public class LoginController : ApiControllerBase<LoginController>
{
    public LoginController(ISender sender, ILogger<LoginController> logger)
        : base(sender, logger) { }

    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommand command)
    {
        try
        {
            var result = await _sender.Send(command);
            _logger.LogInformation("Login realizado com sucesso para usuario {Usuario}", command.Usuario);
            return Ok(result);
        }
        catch (LoginException ex)
        {
            _logger.LogError(ex, "Erro ao realizar login para usuario {Usuario}", command.Usuario);
            return StatusCode(ex.StatusCode, ex.Message);
        }
    }
}
