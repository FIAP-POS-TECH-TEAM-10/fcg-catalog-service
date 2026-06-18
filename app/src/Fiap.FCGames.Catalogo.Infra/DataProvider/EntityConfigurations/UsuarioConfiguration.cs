using Fiap.FCGames.Catalogo.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.EntityConfigurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario");

            builder.HasKey(e => e.Id)
                .HasName("PRIMARY");

            builder.Property(e => e.Id)
                .HasColumnName("id_usuario")
                .HasConversion(id => id.Value, value => new UsuarioId(value))
                .ValueGeneratedNever();

            builder.Property(e => e.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.NomeUsuario)
                .HasColumnName("nome_usuario")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Senha)
                .HasColumnName("senha")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.DataCadastro)
                .HasColumnName("data_cadastro")
                .IsRequired();

            builder.Property(e => e.IdTipoAcesso)
                .HasColumnName("id_tipo_acesso")
                .IsRequired();

            builder.Ignore(e => e.TipoAcesso);

            builder.HasOne(d => d.TipoAcessoUsuario)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTipoAcesso)
                .HasPrincipalKey(p => p.Id);
        }
    }
}