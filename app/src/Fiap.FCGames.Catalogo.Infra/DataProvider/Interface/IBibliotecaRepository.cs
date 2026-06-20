using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateBiblioteca;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;

public interface IBibliotecaRepository
{
    Task<Biblioteca?> ObterPorUsuarioIdAsync(Guid usuarioId);
    Task<Biblioteca?> ObterComItensPorUsuarioIdAsync(Guid usuarioId);
    Task<bool> UsuarioPossuiJogoAsync(Guid usuarioId, Guid jogoId);
    void Adicionar(Biblioteca biblioteca);
    void AdicionarItem(ItemBiblioteca item);
}
