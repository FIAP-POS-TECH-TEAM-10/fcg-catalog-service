using Fiap.FCGames.Catalogo.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.FCGames.Catalogo.Infra.DataProvider.EntityConfigurations
{
    public class TipoAcessoUsuarioConfiguration : IEntityTypeConfiguration<TipoAcessoUsuario>
    {
        public void Configure(EntityTypeBuilder<TipoAcessoUsuario> builder)
        {
            builder.ToTable("tipo_acesso_usuario");

            builder.HasKey(e => e.Id)
                .HasName("PRIMARY");

            builder.Property(e => e.Id)
                .HasColumnName("id_tipo_acesso")
                .ValueGeneratedNever();

            builder.Property(e => e.Descricao)
                .HasColumnName("descricao")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Role)
                .HasColumnName("role")
                .IsRequired()
                .HasMaxLength(50);

            builder.HasData(
                new TipoAcessoUsuario { Id = 1, Descricao = "Administrador", Role = "Admin" },
                new TipoAcessoUsuario { Id = 2, Descricao = "Usuário", Role = "User" }
            );
        }
    }
}
