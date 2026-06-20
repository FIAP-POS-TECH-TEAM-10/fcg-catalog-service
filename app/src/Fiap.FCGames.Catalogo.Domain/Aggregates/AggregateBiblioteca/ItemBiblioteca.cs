namespace Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateBiblioteca;

public class ItemBiblioteca
{
    public Guid Id { get; set; }
    public Guid BibliotecaId { get; set; }
    public Guid JogoId { get; set; }
    public DateTime DataAdicao { get; set; }

    public Biblioteca? Biblioteca { get; set; }
}
