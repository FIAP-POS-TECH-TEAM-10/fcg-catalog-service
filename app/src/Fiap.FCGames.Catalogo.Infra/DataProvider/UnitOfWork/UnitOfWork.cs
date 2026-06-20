using Fiap.FCGames.Catalogo.Infra.DataProvider.Contexto;
using Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly FcGamesContexto _context;
    public IJogoRepository JogoRepository { get; }
    public IBibliotecaRepository BibliotecaRepository { get; }
    public IPedidoRepository PedidoRepository { get; }

    public UnitOfWork(
        FcGamesContexto context,
        IJogoRepository jogoRepository,
        IBibliotecaRepository bibliotecaRepository,
        IPedidoRepository pedidoRepository)
    {
        _context = context;
        JogoRepository = jogoRepository;
        BibliotecaRepository = bibliotecaRepository;
        PedidoRepository = pedidoRepository;
    }

    public Task<int> CommitAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
