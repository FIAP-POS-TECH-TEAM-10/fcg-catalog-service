using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.BibliotecaJogos.BuscarBibliotecaJogos;

public record BuscarBibliotecaJogosQuery(Guid UsuarioId) : IRequest<BuscarBibliotecaJogosResponse?>;
