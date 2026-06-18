namespace Fiap.FCGames.Catalogo.Domain.Interface.Service;

public interface IPasswordHasherService
{
    string GerarHash(string senha);
    bool Verificar(string senhaTexto, string senhaHash);
}
