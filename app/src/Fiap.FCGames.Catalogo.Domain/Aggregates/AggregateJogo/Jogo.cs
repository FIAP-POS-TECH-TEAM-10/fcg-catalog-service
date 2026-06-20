namespace Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateJogo;

public class Jogo
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public DateTime DataCadastro { get; set; }
}
