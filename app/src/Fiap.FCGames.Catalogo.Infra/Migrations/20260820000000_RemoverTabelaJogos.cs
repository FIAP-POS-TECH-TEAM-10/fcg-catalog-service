using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.FCGames.Catalogo.Infra.Migrations
{
    /// <inheritdoc />
    public partial class RemoverTabelaJogos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Jogo passa a ser persistido no DynamoDB; a tabela relacional deixa de existir.
            migrationBuilder.DropTable(
                name: "Jogos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jogos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogos", x => x.Id);
                });
        }
    }
}
