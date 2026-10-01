using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Transacoes.Common;

namespace SAVi.Application.Transacoes.Queries.ListarTransacoes;

public record ListarTransacoesQuery(int Mes, int Ano) : IRequest<IReadOnlyList<TransacaoDto>>;

public class ListarTransacoesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<ListarTransacoesQuery, IReadOnlyList<TransacaoDto>>
{
    public async Task<IReadOnlyList<TransacaoDto>> Handle(
        ListarTransacoesQuery request, CancellationToken cancellationToken)
    {
        var usuarioId = currentUser.UsuarioId;
        var inicio = new DateOnly(request.Ano, request.Mes, 1);
        var fim = inicio.AddMonths(1);

        var transacoes = await context.Transacoes
            .Include(t => t.Categoria)
            .Where(t => t.UsuarioId == usuarioId)
            .PossivelmenteEntre(inicio, fim)
            .ToListAsync(cancellationToken);

        return transacoes
            .Select(t => (Transacao: t, Ocorrencia: t.OcorrenciaNoMes(request.Ano, request.Mes)))
            .Where(x => x.Ocorrencia is not null)
            .Select(x => TransacaoDto.DeEntidade(x.Transacao, x.Transacao.Categoria!, x.Ocorrencia!.Value))
            .OrderByDescending(dto => dto.Data)
            .ToList();
    }
}
