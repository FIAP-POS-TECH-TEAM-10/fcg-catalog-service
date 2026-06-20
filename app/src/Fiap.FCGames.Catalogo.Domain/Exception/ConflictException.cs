namespace Fiap.FCGames.Catalogo.Domain.Exception;

public class ConflictException : System.Exception
{
    public ConflictException(string message) : base(message) { }
}
