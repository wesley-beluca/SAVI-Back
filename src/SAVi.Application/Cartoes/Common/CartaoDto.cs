using SAVi.Domain.Entities;

namespace SAVi.Application.Cartoes.Common;

/// <param name="FaturaAtual">Fatura atual: fatura inicial + compras no cartão já pagas/lançadas.</param>
/// <param name="FaturaInicial">Valor informado no cadastro do cartão.</param>
public record CartaoDto(
    Guid Id, string Nome, string Banco, decimal FaturaAtual, int DiaFechamento, int DiaVencimento, decimal FaturaInicial)
{
    /// <param name="variacao">Variação líquida das movimentações do cartão (despesas pagas subtraem), vinda de
    /// <c>MovimentacaoPorContaCalculator</c>; a fatura cresce com as compras, então o sinal é invertido.</param>
    public static CartaoDto DeEntidade(Cartao cartao, decimal variacao = 0) => new(
        cartao.Id, cartao.Nome, cartao.Banco, cartao.FaturaAtual - variacao, cartao.DiaFechamento, cartao.DiaVencimento,
        cartao.FaturaAtual);
}
