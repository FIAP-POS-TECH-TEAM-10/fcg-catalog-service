using MediatR;

namespace Fiap.FCGames.Catalogo.Application.Queries.Desejos.ListarDesejos;

public record ListarDesejosQuery(Guid UsuarioId) : IRequest<List<DesejoResponse>>;
