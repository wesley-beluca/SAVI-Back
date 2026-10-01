using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Transacoes.Common;
using SAVi.Domain.Enums;

namespace SAVi.Application.Transacoes.Queries.ObterEvolucaoMensal;

public record PontoEvolucaoMensalDto(int Ano, int Mes, decimal Receitas, decimal Despesas);

public record ObterEvolucaoMensalQuery(int Meses = 12) : IRequest<IReadOnlyList<PontoEvolucaoMensalDto>>;

public class ObterEvolucaoMensalQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<ObterEvolucaoMensalQuery, IReadOnlyList<PontoEvolucaoMensalDto>>
{
    public async Task<IReadOnlyList<PontoEvolucaoMensalDto>> Handle(
        ObterEvolucaoMensalQuery request, CancellationToken cancellationToken)
    {
        var usuarioId = currentUser.UsuarioId;
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
        var mesesSolicitados = Math.Max(1, request.Meses);
        var inicio = new DateOnly(hoje.Year, hoje.Month, 1).AddMonths(-(mesesSolicitados - 1));

        var fim = inicio.AddMonths(mesesSolicitados);

        var transacoes = await context.Transacoes
            .Where(t => t.UsuarioId == usuarioId)
            .PossivelmenteEntre(inicio, fim)
            .ToListAsync(cancellationToken);

        var pontos = new List<PontoEvolucaoMensalDto>();

        for (var i = 0; i < mesesSolicitados; i++)
        {
            var referencia = inicio.AddMonths(i);

            var doMes = transacoes.Where(t => t.OcorrenciaNoMes(referencia.Year, referencia.Month) is not null).ToList();
            var receitas = doMes.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor);
            var despesas = doMes.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor);

            pontos.Add(new PontoEvolucaoMensalDto(referencia.Year, referencia.Month, receitas, despesas));
        }

        return pontos;
    }
}
