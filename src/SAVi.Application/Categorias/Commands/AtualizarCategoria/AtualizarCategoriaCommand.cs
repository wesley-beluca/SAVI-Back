using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Categorias.Common;
using SAVi.Application.Common.Interfaces;
using SAVi.Domain.Exceptions;

namespace SAVi.Application.Categorias.Commands.AtualizarCategoria;

public record AtualizarCategoriaCommand(Guid Id, string Nome, string Icone, string Cor) : IRequest<CategoriaDto>;

public class AtualizarCategoriaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<AtualizarCategoriaCommand, CategoriaDto>
{
    public async Task<CategoriaDto> Handle(AtualizarCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await context.Categorias
            .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (categoria is null || categoria.UsuarioId != currentUser.UsuarioId)
        {
            throw new NotFoundException(nameof(Domain.Entities.Categoria), request.Id);
        }

        categoria.Nome = request.Nome;
        categoria.Icone = request.Icone;
        categoria.Cor = request.Cor;

        await context.SaveChangesAsync(cancellationToken);

        return CategoriaDto.DeEntidade(categoria);
    }
}
