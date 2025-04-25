using Microsoft.EntityFrameworkCore;
using FuriaAPP.API.Models;

namespace FuriaAPP.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Historia> Historias { get; set; }
        public DbSet<JogoHistorico> jogoHistoricos{ get; set; }
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<Campeonato> Campeonatos{ get; set; }
    }
}