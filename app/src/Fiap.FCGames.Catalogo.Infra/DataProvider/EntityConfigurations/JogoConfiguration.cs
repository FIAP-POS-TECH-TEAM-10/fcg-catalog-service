using Fiap.FCGames.Catalogo.Domain.Aggregates.AggregateJogo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.EntityConfigurations;

public class JogoConfiguration : IEntityTypeConfiguration<Jogo>
{
    public void Configure(EntityTypeBuilder<Jogo> builder)
    {
        builder.ToTable("Jogos");
        builder.HasKey(j => j.Id);
        builder.Property(j => j.Nome).IsRequired().HasMaxLength(200);
        builder.Property(j => j.Descricao).IsRequired().HasMaxLength(1000);
        builder.Property(j => j.Preco).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(j => j.DataCadastro).IsRequired();
    }
}
