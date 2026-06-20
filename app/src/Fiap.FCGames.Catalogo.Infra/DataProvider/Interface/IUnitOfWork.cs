namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;

public interface IUnitOfWork
{
    IJogoRepository JogoRepository { get; }
    IBibliotecaRepository BibliotecaRepository { get; }
    IPedidoRepository PedidoRepository { get; }
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
