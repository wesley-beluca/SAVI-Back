using Microsoft.EntityFrameworkCore;
using SAVi.Domain.Entities;
using SAVi.Domain.Enums;

namespace SAVi.Application.Transacoes.Common;

public static class TransacaoQueryExtensions
{
    /// <summary>
    /// Pré-filtro no banco das transações que podem ter ocorrência entre <paramref name="inicio"/> (inclusivo)
    /// e <paramref name="fim"/> (exclusivo). Recorrentes iniciadas antes do intervalo são incluídas e
    /// devem ser confirmadas em memória com <see cref="Transacao.OcorrenciaNoMes"/>.
    /// </summary>
    public static IQueryable<Transacao> PossivelmenteEntre(this IQueryable<Transacao> transacoes, DateOnly inicio, DateOnly fim)
        => transacoes.Where(t => t.Data < fim && (t.Data >= inicio || t.MesesRecorrencia > 1));

    public static async Task<decimal> SomarDespesasDoMesAsync(
        this IQueryable<Transacao> transacoes, Guid? usuarioId, int ano, int mes, CancellationToken cancellationToken)
    {
        var inicio = new DateOnly(ano, mes, 1);

        var despesas = await transacoes
            .Where(t => t.UsuarioId == usuarioId && t.Tipo == TipoTransacao.Despesa)
            .PossivelmenteEntre(inicio, inicio.AddMonths(1))
            .ToListAsync(cancellationToken);

        return despesas.Where(t => t.OcorrenciaNoMes(ano, mes) is not null).Sum(t => t.Valor);
    }
}
