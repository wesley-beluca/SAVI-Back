using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Domain.Entities;
using SAVi.Infrastructure.Identity;
using SAVi.Infrastructure.Persistence.Seed;

namespace SAVi.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options), IApplicationDbContext
{
    public DbSet<Categoria> Categorias => Set<Categoria>();

    public DbSet<Transacao> Transacoes => Set<Transacao>();

    public DbSet<Orcamento> Orcamentos => Set<Orcamento>();

    public DbSet<Conta> Contas => Set<Conta>();

    public DbSet<Cartao> Cartoes => Set<Cartao>();

    public DbSet<Meta> Metas => Set<Meta>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        builder.Entity<Categoria>().HasData(CategoriasPadraoSeed.Categorias);
    }
}
