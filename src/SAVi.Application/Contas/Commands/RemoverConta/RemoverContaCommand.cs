using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Domain.Entities;
using SAVi.Domain.Exceptions;

namespace SAVi.Application.Contas.Commands.RemoverConta;

public record RemoverContaCommand(Guid Id) : IRequest;

public class RemoverContaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<RemoverContaCommand>
{
    public async Task Handle(RemoverContaCommand request, CancellationToken cancellationToken)
    {
        var conta = await context.Contas.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (conta is null || conta.UsuarioId != currentUser.UsuarioId)
        {
            throw new NotFoundException(nameof(Conta), request.Id);
        }

        context.Contas.Remove(conta);
        await context.SaveChangesAsync(cancellationToken);
    }
}
