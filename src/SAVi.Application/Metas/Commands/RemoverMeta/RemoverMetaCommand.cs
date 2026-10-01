using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Domain.Entities;
using SAVi.Domain.Exceptions;

namespace SAVi.Application.Metas.Commands.RemoverMeta;

public record RemoverMetaCommand(Guid Id) : IRequest;

public class RemoverMetaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<RemoverMetaCommand>
{
    public async Task Handle(RemoverMetaCommand request, CancellationToken cancellationToken)
    {
        var meta = await context.Metas.SingleOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (meta is null || meta.UsuarioId != currentUser.UsuarioId)
        {
            throw new NotFoundException(nameof(Meta), request.Id);
        }

        context.Metas.Remove(meta);
        await context.SaveChangesAsync(cancellationToken);
    }
}
