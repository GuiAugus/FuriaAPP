using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuriaAPP.API.Migrations
{
    /// <inheritdoc />
    public partial class NomeDaMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Noticias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", nullable: true),
                    Conteudo = table.Column<string>(type: "TEXT", nullable: true),
                    DataPublicacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    JogoId = table.Column<int>(type: "INTEGER", nullable: true),
                    CampeonatoId = table.Column<int>(type: "INTEGER", nullable: true),
                    TemporadaId = table.Column<int>(type: "INTEGER", nullable: true),
                    JogoHistoricoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Noticias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Noticias_Campeonatos_CampeonatoId",
                        column: x => x.CampeonatoId,
                        principalTable: "Campeonatos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Noticias_Jogos_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Noticias_Temporadas_TemporadaId",
                        column: x => x.TemporadaId,
                        principalTable: "Temporadas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Noticias_jogoHistoricos_JogoHistoricoId",
                        column: x => x.JogoHistoricoId,
                        principalTable: "jogoHistoricos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Noticias_CampeonatoId",
                table: "Noticias",
                column: "CampeonatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Noticias_JogoHistoricoId",
                table: "Noticias",
                column: "JogoHistoricoId");

            migrationBuilder.CreateIndex(
                name: "IX_Noticias_JogoId",
                table: "Noticias",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Noticias_TemporadaId",
                table: "Noticias",
                column: "TemporadaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Noticias");
        }
    }
}
