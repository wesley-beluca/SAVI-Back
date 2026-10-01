using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Domain.Entities;
using SAVi.Domain.Exceptions;

namespace SAVi.Application.Cartoes.Commands.RemoverCartao;

public record RemoverCartaoCommand(Guid Id) : IRequest;

public class RemoverCartaoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<RemoverCartaoCommand>
{
    public async Task Handle(RemoverCartaoCommand request, CancellationToken cancellationToken)
    {
        var cartao = await context.Cartoes.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (cartao is null || cartao.UsuarioId != currentUser.UsuarioId)
        {
            throw new NotFoundException(nameof(Cartao), request.Id);
        }

        context.Cartoes.Remove(cartao);
        await context.SaveChangesAsync(cancellationToken);
    }
}
