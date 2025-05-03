using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuriaAPP.API.Migrations
{
    /// <inheritdoc />
    public partial class ReinializandoDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adversarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adversarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Historias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", nullable: false),
                    Conteudo = table.Column<string>(type: "TEXT", nullable: false),
                    DataPublicacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historias", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    CPF = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Senha = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Campeonatos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    JogoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campeonatos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campeonatos_Jogos_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "jogoJogoInteresse",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    JogoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jogoJogoInteresse", x => new { x.UsuarioId, x.JogoId });
                    table.ForeignKey(
                        name: "FK_jogoJogoInteresse_Jogos_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_jogoJogoInteresse_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Temporadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    CampeonatoId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFim = table.Column<DateTime>(type: "TEXT", nullable: true),
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
                    Resultado = table.Column<string>(type: "TEXT", nullable: false),
                    DataJogo = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CampeonatoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Campeonato = table.Column<string>(type: "TEXT", nullable: false),
                    TemporadaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jogoHistoricos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_jogoHistoricos_Temporadas_TemporadaId",
                        column: x => x.TemporadaId,
                        principalTable: "Temporadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Campeonatos_JogoId",
                table: "Campeonatos",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_jogoHistoricos_TemporadaId",
                table: "jogoHistoricos",
                column: "TemporadaId");

            migrationBuilder.CreateIndex(
                name: "IX_jogoJogoInteresse_JogoId",
                table: "jogoJogoInteresse",
                column: "JogoId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Temporadas_CampeonatoId",
                table: "Temporadas",
                column: "CampeonatoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adversarios");

            migrationBuilder.DropTable(
                name: "Historias");

            migrationBuilder.DropTable(
                name: "jogoJogoInteresse");

            migrationBuilder.DropTable(
                name: "Noticias");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "jogoHistoricos");

            migrationBuilder.DropTable(
                name: "Temporadas");

            migrationBuilder.DropTable(
                name: "Campeonatos");

            migrationBuilder.DropTable(
                name: "Jogos");
        }
    }
}
