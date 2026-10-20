using Microsoft.EntityFrameworkCore;
using SAVi.Domain.Entities;
using SAVi.Domain.Enums;

namespace SAVi.Application.Transacoes.Common;

/// <summary>
/// Calcula o efeito das movimentações do usuário sobre cada conta / cartão (identificados pelo nome gravado em
/// <see cref="Transacao.Conta"/>). Receitas contam a partir da data; despesas só depois de marcadas como pagas.
/// </summary>
public static class MovimentacaoPorContaCalculator
{
    public const string SufixoCredito = " (Crédito)";

    public static string NomeDoCartao(string nomeCartao) => $"{nomeCartao}{SufixoCredito}";

    /// <summary>Variação líquida por nome de conta: receitas somam, despesas pagas subtraem.</summary>
    public static async Task<Dictionary<string, decimal>> CalcularAsync(
        IQueryable<Transacao> transacoes, Guid? usuarioId, CancellationToken cancellationToken)
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);

        var lista = await transacoes
            .Include(t => t.Pagamentos)
            .Where(t => t.UsuarioId == usuarioId)
            .ToListAsync(cancellationToken);

        var variacao = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

        foreach (var transacao in lista)
        {
            var total = 0m;

            for (var ocorrencia = 1; ocorrencia <= transacao.MesesRecorrencia; ocorrencia++)
            {
                if (transacao.DataDaOcorrencia(ocorrencia) > hoje)
                {
                    break;
                }

                if (transacao.Tipo == TipoTransacao.Receita)
                {
                    total += transacao.Valor;
                }
                else if (transacao.PagamentoDaOcorrencia(ocorrencia) is not null)
                {
                    total -= transacao.Valor;
                }
            }

            if (total != 0)
            {
                variacao[transacao.Conta] = variacao.GetValueOrDefault(transacao.Conta) + total;
            }
        }

        return variacao;
    }
}
