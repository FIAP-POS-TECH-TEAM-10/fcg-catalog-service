using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.Jogos.BuscarJogos;

/// <summary>
/// Query para buscar jogos com suporte a fuzzy search.
/// Busca no índice Elasticsearch para retornar resultados ordenados por relevância.
/// </summary>
public record BuscarJogosQuery(
    string Termo,
    bool Fuzzy = true,
    int Tamanho = 20
) : IRequest<List<BuscarJogosResponse>>;
