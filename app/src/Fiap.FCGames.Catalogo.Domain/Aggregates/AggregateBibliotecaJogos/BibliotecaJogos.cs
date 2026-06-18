namespace Fiap.FCGames.Catalogo.Domain.Aggregates
{
    public class BibliotecaJogos
    {
        public BibliotecaJogosId Id { get; set; }
        public DateTime DataCriacao { get; set; }

        public UsuarioId IdUsuario { get; set; }
        public virtual Usuario? Usuario { get; set; }
    }
}
