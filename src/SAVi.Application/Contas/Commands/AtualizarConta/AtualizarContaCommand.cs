using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Contas.Common;
using SAVi.Domain.Entities;
using SAVi.Domain.Exceptions;

namespace SAVi.Application.Contas.Commands.AtualizarConta;

public record AtualizarContaCommand(Guid Id, string Nome, string Banco, decimal Saldo) : IRequest<ContaDto>;

public class AtualizarContaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<AtualizarContaCommand, ContaDto>
{
    public async Task<ContaDto> Handle(AtualizarContaCommand request, CancellationToken cancellationToken)
    {
        var conta = await context.Contas.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (conta is null || conta.UsuarioId != currentUser.UsuarioId)
        {
            throw new NotFoundException(nameof(Conta), request.Id);
        }

        conta.Nome = request.Nome;
        conta.Banco = request.Banco;
        conta.Saldo = request.Saldo;

        await context.SaveChangesAsync(cancellationToken);

        return ContaDto.DeEntidade(conta);
    }
}
