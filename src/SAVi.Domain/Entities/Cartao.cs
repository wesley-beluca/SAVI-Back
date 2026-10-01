using SAVi.Domain.Common;

namespace SAVi.Domain.Entities;

public class Cartao : BaseEntity
{
    public required string Nome { get; set; }

    public required string Banco { get; set; }

    public decimal FaturaAtual { get; set; }

    public required int DiaFechamento { get; set; }

    public required int DiaVencimento { get; set; }

    public required Guid UsuarioId { get; set; }
}
