using SAVi.Domain.Entities;

namespace SAVi.Application.Cartoes.Common;

public record CartaoDto(Guid Id, string Nome, string Banco, decimal FaturaAtual, int DiaFechamento, int DiaVencimento)
{
    public static CartaoDto DeEntidade(Cartao cartao) => new(
        cartao.Id, cartao.Nome, cartao.Banco, cartao.FaturaAtual, cartao.DiaFechamento, cartao.DiaVencimento);
}
