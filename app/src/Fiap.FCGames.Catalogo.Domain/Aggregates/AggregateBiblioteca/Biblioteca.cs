namespace Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateBiblioteca;

public class Biblioteca
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public DateTime CriadaEm { get; set; }

    public ICollection<ItemBiblioteca> Itens { get; set; } = new List<ItemBiblioteca>();
}
