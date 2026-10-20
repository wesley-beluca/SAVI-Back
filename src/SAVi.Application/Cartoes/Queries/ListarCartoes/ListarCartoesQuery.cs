using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Cartoes.Common;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Transacoes.Common;

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

        var variacao = await MovimentacaoPorContaCalculator.CalcularAsync(context.Transacoes, usuarioId, cancellationToken);

        return cartoes
            .Select(c => CartaoDto.DeEntidade(c, variacao.GetValueOrDefault(MovimentacaoPorContaCalculator.NomeDoCartao(c.Nome))))
            .ToList();
    }
}
