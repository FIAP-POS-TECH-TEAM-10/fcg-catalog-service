using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;
using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.Usuario.ListarUsuarios;

public class ListarUsuariosQueryHandler : IRequestHandler<ListarUsuariosQuery, IEnumerable<ListaUsuariosDto>>
{
    private readonly IUsuarioRepository _usuarioRepo;

    public ListarUsuariosQueryHandler(IUsuarioRepository usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
    }

    public async Task<IEnumerable<ListaUsuariosDto>> Handle(ListarUsuariosQuery request, CancellationToken cancellationToken)
    {
        var usuarios = await _usuarioRepo.ObterTodosAsync();
        return usuarios.Select(u => new ListaUsuariosDto
        {
            Id = u.Id.Value,
            Nome = u.Nome,
            Email = u.Email,
            NomeUsuario = u.NomeUsuario
        }).ToList();
    }
}
