using Microsoft.EntityFrameworkCore;
using FuriaAPP.API.Models;
using FuriaAPP.API.Models.Usuario;

namespace FuriaAPP.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Usuario> Usuarios{ get; set; }
        public DbSet<Historia> Historias { get; set; }
        public DbSet<Noticia> Noticias { get; set; }
        public DbSet<JogoHistorico> jogoHistoricos{ get; set; }
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<UsuarioJogoInteresse> jogoJogoInteresse { get; set; }
        public DbSet<Campeonato> Campeonatos{ get; set; }
        public DbSet<Temporada> Temporadas{ get; set; }
        public DbSet<Adversario> Adversarios{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsuarioJogoInteresse>()
                .HasKey(uj => new { uj.UsuarioId, uj.JogoId });
        }

    }
}