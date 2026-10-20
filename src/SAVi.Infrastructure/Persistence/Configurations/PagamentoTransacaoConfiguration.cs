using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAVi.Domain.Entities;

namespace SAVi.Infrastructure.Persistence.Configurations;

public class PagamentoTransacaoConfiguration : IEntityTypeConfiguration<PagamentoTransacao>
{
    public void Configure(EntityTypeBuilder<PagamentoTransacao> builder)
    {
        builder.ToTable("PagamentosTransacoes");

        builder.HasOne<Transacao>()
            .WithMany(t => t.Pagamentos)
            .HasForeignKey(p => p.TransacaoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => new { p.TransacaoId, p.Ocorrencia }).IsUnique();
    }
}
