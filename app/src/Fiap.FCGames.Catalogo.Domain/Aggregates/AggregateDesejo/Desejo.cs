namespace Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateDesejo;

public class Desejo
{
    public Guid UsuarioId { get; set; }
    public Guid JogoId { get; set; }
    public DateTime AdicionadoEm { get; set; }
}
