using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuriaAPP.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFim",
                table: "Temporadas",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "JogoId",
                table: "Campeonatos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Campeonatos_JogoId",
                table: "Campeonatos",
                column: "JogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campeonatos_Jogos_JogoId",
                table: "Campeonatos",
                column: "JogoId",
                principalTable: "Jogos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campeonatos_Jogos_JogoId",
                table: "Campeonatos");

            migrationBuilder.DropIndex(
                name: "IX_Campeonatos_JogoId",
                table: "Campeonatos");

            migrationBuilder.DropColumn(
                name: "JogoId",
                table: "Campeonatos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFim",
                table: "Temporadas",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
