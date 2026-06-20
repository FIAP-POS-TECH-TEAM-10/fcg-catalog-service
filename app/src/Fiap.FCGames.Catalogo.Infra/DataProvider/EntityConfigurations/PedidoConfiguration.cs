using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregatePedido;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.EntityConfigurations;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.UsuarioId).IsRequired();
        builder.Property(p => p.JogoId).IsRequired();
        builder.Property(p => p.Preco).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.CriadoEm).IsRequired();
    }
}
