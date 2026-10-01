using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Exceptions;
using SAVi.Application.Common.Interfaces;
using SAVi.Domain.Exceptions;

namespace SAVi.Application.Categorias.Commands.RemoverCategoria;

public record RemoverCategoriaCommand(Guid Id) : IRequest;

public class RemoverCategoriaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<RemoverCategoriaCommand>
{
    public async Task Handle(RemoverCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await context.Categorias
            .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (categoria is null || categoria.UsuarioId != currentUser.UsuarioId)
        {
            throw new NotFoundException(nameof(Domain.Entities.Categoria), request.Id);
        }

        var possuiTransacoes = await context.Transacoes
            .AnyAsync(t => t.CategoriaId == request.Id, cancellationToken);

        if (possuiTransacoes)
        {
            throw new ValidationException(
            [
                new ValidationFailure(nameof(request.Id),
                    "Esta categoria possui movimentações vinculadas e não pode ser excluída.")
            ]);
        }

        context.Categorias.Remove(categoria);
        await context.SaveChangesAsync(cancellationToken);
    }
}
