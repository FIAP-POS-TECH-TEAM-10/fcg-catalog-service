using Fiap.FCGames.Catalogo.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.EntityConfigurations
{
    public class BibliotecaJogosConfiguration : IEntityTypeConfiguration<BibliotecaJogos>
    {
        public void Configure(EntityTypeBuilder<BibliotecaJogos> builder)
        {
            builder.ToTable("bibliotecas_usuario");

            builder.HasKey(e => e.Id)
                .HasName("PRIMARY");

            builder.Property(e => e.Id)
                .HasColumnName("id_biblioteca")
                .HasConversion(id => id.Value, value => new BibliotecaJogosId(value))
                .ValueGeneratedNever();

            builder.Property(e => e.DataCriacao)
                .HasColumnName("data_criacao")
                .IsRequired();

            builder.Property(e => e.IdUsuario)
                .HasColumnName("id_usuario")
                .HasConversion(id => id.Value, value => new UsuarioId(value))
                .IsRequired();

            builder.HasOne(b => b.Usuario)
                .WithOne(u => u.Biblioteca)
                .HasForeignKey<BibliotecaJogos>(b => b.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
