using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Desejos.RemoverDesejo;

public record RemoverDesejoCommand(Guid UsuarioId, Guid JogoId) : IRequest<Unit>;
