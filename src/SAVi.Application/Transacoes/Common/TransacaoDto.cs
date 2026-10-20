using SAVi.Domain.Entities;
using SAVi.Domain.Enums;

namespace SAVi.Application.Transacoes.Common;

public record TransacaoDto(
    Guid Id,
    string Descricao,
    decimal Valor,
    DateOnly Data,
    TipoTransacao Tipo,
    bool Recorrente,
    string Conta,
    int Parcelas,
    DateOnly DataInicio,
    int MesesRecorrencia,
    int Ocorrencia,
    bool Pago,
    DateTime? PagoEm,
    Guid CategoriaId,
    string CategoriaNome,
    string CategoriaIcone,
    string CategoriaCor)
{
    /// <remarks>Requer <see cref="Transacao.Pagamentos"/> carregado para preencher <see cref="Pago"/>.</remarks>
    public static TransacaoDto DeEntidade(Transacao transacao, Categoria categoria, int ocorrencia = 1)
    {
        var pagamento = transacao.PagamentoDaOcorrencia(ocorrencia);

        return new(
            transacao.Id,
            transacao.Descricao,
            transacao.Valor,
            transacao.DataDaOcorrencia(ocorrencia),
            transacao.Tipo,
            transacao.Recorrente,
            transacao.Conta,
            transacao.Parcelas,
            transacao.Data,
            transacao.MesesRecorrencia,
            ocorrencia,
            pagamento is not null,
            pagamento?.CreatedAtUtc,
            categoria.Id,
            categoria.Nome,
            categoria.Icone,
            categoria.Cor);
    }
}
