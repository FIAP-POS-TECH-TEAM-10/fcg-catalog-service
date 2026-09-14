using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateDesejo;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;

public interface IDesejoRepository
{
    Task<List<Desejo>> ListarPorUsuarioAsync(Guid usuarioId);
    Task<Desejo?> ObterAsync(Guid usuarioId, Guid jogoId);
    Task AdicionarAsync(Desejo desejo);
    Task RemoverAsync(Guid usuarioId, Guid jogoId);
}
