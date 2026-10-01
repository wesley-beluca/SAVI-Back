using SAVi.Domain.Common;

namespace SAVi.Domain.Entities;

public class Meta : BaseEntity
{
    public required string Nome { get; set; }

    public required string Icone { get; set; }

    public required string Cor { get; set; }

    public decimal ValorAtual { get; set; }

    public required decimal ValorAlvo { get; set; }

    public required Guid UsuarioId { get; set; }
}
