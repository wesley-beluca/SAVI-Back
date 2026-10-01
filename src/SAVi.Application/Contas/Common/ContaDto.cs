using SAVi.Domain.Entities;

namespace SAVi.Application.Contas.Common;

public record ContaDto(Guid Id, string Nome, string Banco, decimal Saldo)
{
    public static ContaDto DeEntidade(Conta conta) => new(conta.Id, conta.Nome, conta.Banco, conta.Saldo);
}
