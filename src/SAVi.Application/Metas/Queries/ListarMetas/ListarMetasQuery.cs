using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Metas.Common;

namespace SAVi.Application.Metas.Queries.ListarMetas;

public record ListarMetasQuery : IRequest<IReadOnlyList<MetaDto>>;

public class ListarMetasQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<ListarMetasQuery, IReadOnlyList<MetaDto>>
{
    public async Task<IReadOnlyList<MetaDto>> Handle(ListarMetasQuery request, CancellationToken cancellationToken)
    {
        var usuarioId = currentUser.UsuarioId;

        var metas = await context.Metas
            .Where(m => m.UsuarioId == usuarioId)
            .OrderBy(m => m.Nome)
            .ToListAsync(cancellationToken);

        return metas.Select(MetaDto.DeEntidade).ToList();
    }
}
