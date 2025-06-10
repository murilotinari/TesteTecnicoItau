using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Domain.Entities;

namespace TesteTecnicoItau.Infrastructure.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UsuarioEntity> Usuarios { get; set; }
    public DbSet<AtivoEntity> Ativos { get; set; }
    public DbSet<OperacaoEntity> Operacoes { get; set; }
    public DbSet<PosicaoEntity> Posicoes { get; set; }
    public DbSet<CotacaoEntity> Cotacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PosicaoEntity>()
            .HasIndex(p => new { p.UsuarioId, p.AtivoId })
            .IsUnique();

        modelBuilder.Entity<OperacaoEntity>()
            .HasIndex(o => new { o.UsuarioId, o.AtivoId, o.DataHora });
    }
}
