using SAVi.Domain.Common;

namespace SAVi.Domain.Entities;

/// <summary>
/// Marca uma ocorrência de uma despesa como paga. Como a recorrência é um único registro de
/// <see cref="Transacao"/>, o pagamento é guardado por ocorrência. A data do pagamento é <see cref="BaseEntity.CreatedAtUtc"/>.
/// </summary>
public class PagamentoTransacao : BaseEntity
{
    public required Guid TransacaoId { get; set; }

    public required int Ocorrencia { get; set; }
}
