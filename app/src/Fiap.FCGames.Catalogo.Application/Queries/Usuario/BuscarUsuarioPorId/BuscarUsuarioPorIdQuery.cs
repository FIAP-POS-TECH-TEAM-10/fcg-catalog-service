using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.Usuario.BuscarUsuarioPorId;

public record BuscarUsuarioPorIdQuery(Guid Id) : IRequest<DetalhesUsuarioDto>;
