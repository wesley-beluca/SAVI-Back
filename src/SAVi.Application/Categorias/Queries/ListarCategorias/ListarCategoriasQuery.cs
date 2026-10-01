using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Categorias.Common;
using SAVi.Application.Common.Interfaces;

namespace SAVi.Application.Categorias.Queries.ListarCategorias;

public record ListarCategoriasQuery : IRequest<IReadOnlyList<CategoriaDto>>;

public class ListarCategoriasQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<ListarCategoriasQuery, IReadOnlyList<CategoriaDto>>
{
    public async Task<IReadOnlyList<CategoriaDto>> Handle(
        ListarCategoriasQuery request, CancellationToken cancellationToken)
    {
        var usuarioId = currentUser.UsuarioId;

        var categorias = await context.Categorias
            .Where(c => c.UsuarioId == null || c.UsuarioId == usuarioId)
            .OrderBy(c => c.Nome)
            .ToListAsync(cancellationToken);

        return categorias.Select(CategoriaDto.DeEntidade).ToList();
    }
}
