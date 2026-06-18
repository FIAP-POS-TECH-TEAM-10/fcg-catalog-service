namespace Fiap.FCGames.Catalogo.Domain.Exception
{
    public class LoginException : System.Exception
    {
        public int StatusCode { get; set; }
        public LoginException() { }

        public LoginException(string message, int statusCode) : base(message) { StatusCode = statusCode; }

        public LoginException(string message, int statusCode, System.Exception innerException) : base(message, innerException) { StatusCode = statusCode; }
    }
}
