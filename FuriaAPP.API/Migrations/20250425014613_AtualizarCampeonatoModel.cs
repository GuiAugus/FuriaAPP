using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuriaAPP.API.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarCampeonatoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campeonatos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFim = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campeonatos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "jogoHistoricos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JogoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Jogo = table.Column<string>(type: "TEXT", nullable: false),
                    AdversarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    Adversario = table.Column<string>(type: "TEXT", nullable: false),
                    PontuacaoFuria = table.Column<int>(type: "INTEGER", nullable: false),
                    PontuacaoAdversario = table.Column<int>(type: "INTEGER", nullable: false),
                    DataJogo = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CampeonatoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Campeonato = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jogoHistoricos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Campeonatos");

            migrationBuilder.DropTable(
                name: "jogoHistoricos");

            migrationBuilder.DropTable(
                name: "Jogos");
        }
    }
}
