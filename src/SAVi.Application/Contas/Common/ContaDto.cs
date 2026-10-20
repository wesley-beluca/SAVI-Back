using SAVi.Domain.Entities;

namespace SAVi.Application.Contas.Common;

/// <param name="Saldo">Saldo atual: saldo inicial + receitas − despesas pagas.</param>
/// <param name="SaldoInicial">Valor informado no cadastro da conta.</param>
public record ContaDto(Guid Id, string Nome, string Banco, decimal Saldo, decimal SaldoInicial)
{
    public static ContaDto DeEntidade(Conta conta, decimal variacao = 0)
        => new(conta.Id, conta.Nome, conta.Banco, conta.Saldo + variacao, conta.Saldo);
}
