using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAVi.Domain.Entities;

namespace SAVi.Infrastructure.Persistence.Configurations;

public class TransacaoConfiguration : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.ToTable("Transacoes");

        builder.Property(t => t.Descricao).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Valor).HasPrecision(18, 2);
        builder.Property(t => t.Conta).HasMaxLength(100).IsRequired();

        builder.HasOne(t => t.Categoria)
            .WithMany(c => c.Transacoes)
            .HasForeignKey(t => t.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => new { t.UsuarioId, t.Data });
    }
}
