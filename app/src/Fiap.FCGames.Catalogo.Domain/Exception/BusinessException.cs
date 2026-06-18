namespace Fiap.FCGames.Catalogo.Domain.Exception;

public class BusinessException : System.Exception
{
    public BusinessException(string message) : base(message) { }
    public BusinessException(string message, System.Exception innerException) : base(message, innerException) { }
}
