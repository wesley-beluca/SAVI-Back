using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAVi.Domain.Entities;

namespace SAVi.Infrastructure.Persistence.Configurations;

public class MetaConfiguration : IEntityTypeConfiguration<Meta>
{
    public void Configure(EntityTypeBuilder<Meta> builder)
    {
        builder.ToTable("Metas");

        builder.Property(m => m.Nome).HasMaxLength(100).IsRequired();
        builder.Property(m => m.Icone).HasMaxLength(50).IsRequired();
        builder.Property(m => m.Cor).HasMaxLength(7).IsRequired();
        builder.Property(m => m.ValorAtual).HasPrecision(18, 2);
        builder.Property(m => m.ValorAlvo).HasPrecision(18, 2);

        builder.HasIndex(m => m.UsuarioId);
    }
}
