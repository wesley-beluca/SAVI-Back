using SAVi.Domain.Common;

namespace SAVi.Domain.Entities;

public class Conta : BaseEntity
{
    public required string Nome { get; set; }

    public required string Banco { get; set; }

    public decimal Saldo { get; set; }

    public required Guid UsuarioId { get; set; }
}
