using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Contas.Common;

namespace SAVi.Application.Contas.Queries.ListarContas;

public record ListarContasQuery : IRequest<IReadOnlyList<ContaDto>>;

public class ListarContasQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<ListarContasQuery, IReadOnlyList<ContaDto>>
{
    public async Task<IReadOnlyList<ContaDto>> Handle(ListarContasQuery request, CancellationToken cancellationToken)
    {
        var usuarioId = currentUser.UsuarioId;

        var contas = await context.Contas
            .Where(c => c.UsuarioId == usuarioId)
            .OrderBy(c => c.Nome)
            .ToListAsync(cancellationToken);

        return contas.Select(ContaDto.DeEntidade).ToList();
    }
}
