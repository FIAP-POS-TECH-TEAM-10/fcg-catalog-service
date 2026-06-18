using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Login;

public record LoginCommand(string Usuario, string Senha) : IRequest<UsuarioLogadoDto>;
