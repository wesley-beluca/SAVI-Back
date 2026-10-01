using SAVi.Domain.Common;

namespace SAVi.Domain.Entities;

public class Orcamento : BaseEntity
{
    public required decimal RendaPrevista { get; set; }

    public required decimal LimitePlanejado { get; set; }

    public required int Mes { get; set; }

    public required int Ano { get; set; }

    public required Guid UsuarioId { get; set; }
}
