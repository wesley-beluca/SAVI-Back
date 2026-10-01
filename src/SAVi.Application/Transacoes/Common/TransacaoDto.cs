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
    Guid CategoriaId,
    string CategoriaNome,
    string CategoriaIcone,
    string CategoriaCor)
{
    public static TransacaoDto DeEntidade(Transacao transacao, Categoria categoria, int ocorrencia = 1) => new(
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
        categoria.Id,
        categoria.Nome,
        categoria.Icone,
        categoria.Cor);
}
