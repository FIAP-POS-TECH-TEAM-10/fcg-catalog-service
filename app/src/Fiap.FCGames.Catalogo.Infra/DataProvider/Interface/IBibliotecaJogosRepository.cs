using Fiap.FCGames.Catalogo.Domain.Aggregates;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.Interface;

public interface IBibliotecaJogosRepository
{
    void Adicionar(BibliotecaJogos biblioteca);
    Task<BibliotecaJogos?> ObterPorUsuarioIdAsync(UsuarioId usuarioId);
}
