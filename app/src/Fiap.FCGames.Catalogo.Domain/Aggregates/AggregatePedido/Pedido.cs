using Fiap.FCGames.Catalogo.Domain.Enums;

namespace Fiap.FCGames.Catalogo.Domain.Aggregates.AggregatePedido;

public class Pedido
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid JogoId { get; set; }
    public decimal Preco { get; set; }
    public StatusPedido Status { get; set; }
    public DateTime CriadoEm { get; set; }
}
