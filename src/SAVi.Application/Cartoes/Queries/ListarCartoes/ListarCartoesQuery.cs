using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Cartoes.Common;
using SAVi.Application.Common.Interfaces;

namespace SAVi.Application.Cartoes.Queries.ListarCartoes;

public record ListarCartoesQuery : IRequest<IReadOnlyList<CartaoDto>>;

public class ListarCartoesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<ListarCartoesQuery, IReadOnlyList<CartaoDto>>
{
    public async Task<IReadOnlyList<CartaoDto>> Handle(ListarCartoesQuery request, CancellationToken cancellationToken)
    {
        var usuarioId = currentUser.UsuarioId;

        var cartoes = await context.Cartoes
            .Where(c => c.UsuarioId == usuarioId)
            .OrderBy(c => c.Nome)
            .ToListAsync(cancellationToken);

        return cartoes.Select(CartaoDto.DeEntidade).ToList();
    }
}
