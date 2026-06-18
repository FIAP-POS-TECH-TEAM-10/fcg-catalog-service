namespace Fiap.FCGames.Catalogo.Domain.Aggregates
{
    public class TipoAcessoUsuario
    {
        public int Id { get; set; }
        public required string Descricao { get; set; }
        public required string Role { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; } = [];
    }
}
