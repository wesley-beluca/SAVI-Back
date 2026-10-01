using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Domain.Entities;

namespace SAVi.Application.Tests;

/// <summary>
/// Implementação mínima de IApplicationDbContext sobre o provider InMemory do EF Core,
/// usada só nos testes de Application para não depender do ApplicationDbContext "real"
/// (que vive em Infrastructure e traz Identity junto).
/// </summary>
public class TestApplicationDbContext : DbContext, IApplicationDbContext
{
    public TestApplicationDbContext(DbContextOptions<TestApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias => Set<Categoria>();

    public DbSet<Transacao> Transacoes => Set<Transacao>();

    public DbSet<Orcamento> Orcamentos => Set<Orcamento>();

    public DbSet<Conta> Contas => Set<Conta>();

    public DbSet<Cartao> Cartoes => Set<Cartao>();

    public DbSet<Meta> Metas => Set<Meta>();

    public static TestApplicationDbContext Criar()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestApplicationDbContext(options);
    }
}
