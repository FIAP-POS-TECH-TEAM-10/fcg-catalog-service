namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
