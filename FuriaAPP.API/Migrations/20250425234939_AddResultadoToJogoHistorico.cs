using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuriaAPP.API.Migrations
{
    /// <inheritdoc />
    public partial class AddResultadoToJogoHistorico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFim",
                table: "Campeonatos");

            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "Campeonatos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Campeonatos");

            migrationBuilder.AddColumn<string>(
                name: "Resultado",
                table: "jogoHistoricos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TemporadaId",
                table: "jogoHistoricos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Temporadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    CampeonatoId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFim = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temporadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Temporadas_Campeonatos_CampeonatoId",
                        column: x => x.CampeonatoId,
                        principalTable: "Campeonatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_jogoHistoricos_TemporadaId",
                table: "jogoHistoricos",
                column: "TemporadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Temporadas_CampeonatoId",
                table: "Temporadas",
                column: "CampeonatoId");

            migrationBuilder.AddForeignKey(
                name: "FK_jogoHistoricos_Temporadas_TemporadaId",
                table: "jogoHistoricos",
                column: "TemporadaId",
                principalTable: "Temporadas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_jogoHistoricos_Temporadas_TemporadaId",
                table: "jogoHistoricos");

            migrationBuilder.DropTable(
                name: "Temporadas");

            migrationBuilder.DropIndex(
                name: "IX_jogoHistoricos_TemporadaId",
                table: "jogoHistoricos");

            migrationBuilder.DropColumn(
                name: "Resultado",
                table: "jogoHistoricos");

            migrationBuilder.DropColumn(
                name: "TemporadaId",
                table: "jogoHistoricos");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFim",
                table: "Campeonatos",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicio",
                table: "Campeonatos",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Campeonatos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
