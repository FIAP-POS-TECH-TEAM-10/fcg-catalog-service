using Fiap.FCGames.Catalogo.Api.Controllers.Shared;
using Fiap.FCGames.Catalogo.Application.Queries.Jogos.BuscarJogos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.FCGames.Catalogo.Api.Controllers;

/// <summary>
/// Controller de busca avançada com Elasticsearch.
/// Endpoints para buscar jogos por termo com suporte a fuzzy search.
/// </summary>
[ApiController]
[Route("buscaJogos")]
public class BuscaController : ApiControllerBase<BuscaController>
{
    public BuscaController(ISender sender, ILogger<BuscaController> logger) : base(sender, logger) { }

    /// <summary>
    /// Busca jogos por termo com suporte a fuzzy search e ordenação por relevância.
    /// </summary>
    /// <param name="q">Termo de busca (obrigatório)</param>
    /// <param name="fuzzy">Ativar busca com tolerância a erros (padrão: true)</param>
    /// <param name="size">Quantidade máxima de resultados (padrão: 20)</param>
    /// <example>
    /// GET /buscaJogos?q=hades&fuzzy=true&size=10
    /// Retorna até 10 jogos que correspondem a "hades" com tolerância a erros de digitação,
    /// ordenados por relevância.
    /// </example>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<BuscarJogosResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BuscarAsync(
        [FromQuery(Name = "q")] string? q,
        [FromQuery] bool fuzzy = true,
        [FromQuery] int size = 20,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return BadRequest(new { erro = "Parâmetro 'q' é obrigatório" });
        }

        if (size <= 0 || size > 100)
        {
            return BadRequest(new { erro = "Parâmetro 'size' deve estar entre 1 e 100" });
        }

        var query = new BuscarJogosQuery(q, fuzzy, size);
        var result = await _sender.Send(query, cancellationToken);

        return Ok(result);
    }
}
