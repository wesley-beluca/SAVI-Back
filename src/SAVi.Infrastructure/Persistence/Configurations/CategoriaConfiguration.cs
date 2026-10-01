using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAVi.Domain.Entities;

namespace SAVi.Infrastructure.Persistence.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

        builder.Property(c => c.Nome).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Icone).HasMaxLength(50).IsRequired();
        builder.Property(c => c.Cor).HasMaxLength(7).IsRequired();

        builder.HasIndex(c => new { c.UsuarioId, c.Nome });
    }
}
