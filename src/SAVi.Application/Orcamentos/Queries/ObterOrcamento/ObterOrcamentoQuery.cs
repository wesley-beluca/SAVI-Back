using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Orcamentos.Common;
using SAVi.Application.Transacoes.Common;

namespace SAVi.Application.Orcamentos.Queries.ObterOrcamento;

public record ObterOrcamentoQuery(int Mes, int Ano) : IRequest<OrcamentoDto>;

public class ObterOrcamentoQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<ObterOrcamentoQuery, OrcamentoDto>
{
    public async Task<OrcamentoDto> Handle(ObterOrcamentoQuery request, CancellationToken cancellationToken)
    {
        var usuarioId = currentUser.UsuarioId;

        var orcamento = await context.Orcamentos
            .SingleOrDefaultAsync(
                o => o.UsuarioId == usuarioId && o.Mes == request.Mes && o.Ano == request.Ano,
                cancellationToken);

        var utilizado = await context.Transacoes
            .SomarDespesasDoMesAsync(usuarioId, request.Ano, request.Mes, cancellationToken);

        return new OrcamentoDto(request.Mes, request.Ano, orcamento?.RendaPrevista ?? 0, orcamento?.LimitePlanejado ?? 0, utilizado);
    }
}
