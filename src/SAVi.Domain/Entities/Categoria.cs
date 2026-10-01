using SAVi.Domain.Common;
using SAVi.Domain.Enums;

namespace SAVi.Domain.Entities;

public class Categoria : BaseEntity
{
    public required string Nome { get; set; }

    public required TipoTransacao Tipo { get; set; }

    public required string Icone { get; set; }

    public required string Cor { get; set; }

    public Guid? UsuarioId { get; set; }

    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}
