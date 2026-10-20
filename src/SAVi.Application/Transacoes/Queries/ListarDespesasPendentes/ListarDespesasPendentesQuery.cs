using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Transacoes.Common;
using SAVi.Domain.Enums;

namespace SAVi.Application.Transacoes.Queries.ListarDespesasPendentes;

/// <summary>
/// Ocorrências de despesas ainda não pagas com vencimento entre <see cref="De"/> e <see cref="Ate"/> (inclusivos).
/// As datas vêm do cliente para respeitar o fuso horário do usuário. Usado pelos lembretes de contas a vencer.
/// </summary>
public record ListarDespesasPendentesQuery(DateOnly De, DateOnly Ate) : IRequest<IReadOnlyList<TransacaoDto>>;

public class ListarDespesasPendentesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<ListarDespesasPendentesQuery, IReadOnlyList<TransacaoDto>>
{
    public async Task<IReadOnlyList<TransacaoDto>> Handle(
        ListarDespesasPendentesQuery request, CancellationToken cancellationToken)
    {
        var usuarioId = currentUser.UsuarioId;

        var transacoes = await context.Transacoes
            .Include(t => t.Categoria)
            .Include(t => t.Pagamentos)
            .Where(t => t.UsuarioId == usuarioId && t.Tipo == TipoTransacao.Despesa)
            .PossivelmenteEntre(request.De, request.Ate.AddDays(1))
            .ToListAsync(cancellationToken);

        return transacoes
            .SelectMany(t => t.OcorrenciasEntre(request.De, request.Ate)
                .Where(ocorrencia => t.PagamentoDaOcorrencia(ocorrencia) is null)
                .Select(ocorrencia => TransacaoDto.DeEntidade(t, t.Categoria!, ocorrencia)))
            .OrderBy(dto => dto.Data)
            .ToList();
    }
}
