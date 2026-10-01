using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAVi.Domain.Entities;

namespace SAVi.Infrastructure.Persistence.Configurations;

public class OrcamentoConfiguration : IEntityTypeConfiguration<Orcamento>
{
    public void Configure(EntityTypeBuilder<Orcamento> builder)
    {
        builder.ToTable("Orcamentos");

        builder.Property(o => o.RendaPrevista).HasPrecision(18, 2);
        builder.Property(o => o.LimitePlanejado).HasPrecision(18, 2);

        builder.HasIndex(o => new { o.UsuarioId, o.Ano, o.Mes }).IsUnique();
    }
}
