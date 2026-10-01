using Microsoft.EntityFrameworkCore;
using SAVi.Domain.Entities;

namespace SAVi.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Categoria> Categorias { get; }

    DbSet<Transacao> Transacoes { get; }

    DbSet<Orcamento> Orcamentos { get; }

    DbSet<Conta> Contas { get; }

    DbSet<Cartao> Cartoes { get; }

    DbSet<Meta> Metas { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
