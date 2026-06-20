using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateBiblioteca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.EntityConfigurations;

public class BibliotecaConfiguration : IEntityTypeConfiguration<Biblioteca>
{
    public void Configure(EntityTypeBuilder<Biblioteca> builder)
    {
        builder.ToTable("Bibliotecas");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.UsuarioId).IsRequired();
        builder.HasIndex(b => b.UsuarioId).IsUnique();
        builder.Property(b => b.CriadaEm).IsRequired();

        builder.HasMany(b => b.Itens)
            .WithOne(i => i.Biblioteca)
            .HasForeignKey(i => i.BibliotecaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
