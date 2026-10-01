using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Domain.Entities;
using SAVi.Domain.Exceptions;

namespace SAVi.Application.Transacoes.Commands.RemoverTransacao;

public record RemoverTransacaoCommand(Guid Id) : IRequest;

public class RemoverTransacaoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<RemoverTransacaoCommand>
{
    public async Task Handle(RemoverTransacaoCommand request, CancellationToken cancellationToken)
    {
        var transacao = await context.Transacoes
            .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (transacao is null || transacao.UsuarioId != currentUser.UsuarioId)
        {
            throw new NotFoundException(nameof(Transacao), request.Id);
        }

        context.Transacoes.Remove(transacao);
        await context.SaveChangesAsync(cancellationToken);
    }
}
