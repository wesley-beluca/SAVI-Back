using MediatR;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Contas.Common;
using SAVi.Domain.Entities;

namespace SAVi.Application.Contas.Commands.CriarConta;

public record CriarContaCommand(string Nome, string Banco, decimal Saldo) : IRequest<ContaDto>;

public class CriarContaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<CriarContaCommand, ContaDto>
{
    public async Task<ContaDto> Handle(CriarContaCommand request, CancellationToken cancellationToken)
    {
        var conta = new Conta
        {
            Nome = request.Nome,
            Banco = request.Banco,
            Saldo = request.Saldo,
            UsuarioId = currentUser.UsuarioId!.Value
        };

        context.Contas.Add(conta);
        await context.SaveChangesAsync(cancellationToken);

        return ContaDto.DeEntidade(conta);
    }
}
