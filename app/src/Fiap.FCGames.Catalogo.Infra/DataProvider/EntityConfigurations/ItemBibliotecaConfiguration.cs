using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateBiblioteca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.EntityConfigurations;

public class ItemBibliotecaConfiguration : IEntityTypeConfiguration<ItemBiblioteca>
{
    public void Configure(EntityTypeBuilder<ItemBiblioteca> builder)
    {
        builder.ToTable("ItensBiblioteca");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.BibliotecaId).IsRequired();
        builder.Property(i => i.JogoId).IsRequired();
        builder.Property(i => i.DataAdicao).IsRequired();
        builder.HasIndex(i => new { i.BibliotecaId, i.JogoId }).IsUnique();
    }
}
