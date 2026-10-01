using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAVi.Domain.Entities;

namespace SAVi.Infrastructure.Persistence.Configurations;

public class ContaConfiguration : IEntityTypeConfiguration<Conta>
{
    public void Configure(EntityTypeBuilder<Conta> builder)
    {
        builder.ToTable("Contas");

        builder.Property(c => c.Nome).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Banco).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Saldo).HasPrecision(18, 2);

        builder.HasIndex(c => c.UsuarioId);
    }
}
