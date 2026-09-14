using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Commands.Desejos.AdicionarDesejo;

public record AdicionarDesejoCommand(Guid UsuarioId, Guid JogoId) : IRequest<AdicionarDesejoResponse>;
