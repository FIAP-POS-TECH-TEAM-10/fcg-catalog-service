using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.Usuario.ListarUsuarios;

public record ListarUsuariosQuery : IRequest<IEnumerable<ListaUsuariosDto>>;
