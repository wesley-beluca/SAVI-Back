using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Cartoes.Common;
using SAVi.Application.Common.Interfaces;
using SAVi.Domain.Entities;
using SAVi.Domain.Exceptions;

namespace SAVi.Application.Cartoes.Commands.AtualizarCartao;

public record AtualizarCartaoCommand(
    Guid Id, string Nome, string Banco, decimal FaturaAtual, int DiaFechamento, int DiaVencimento)
    : IRequest<CartaoDto>;

public class AtualizarCartaoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<AtualizarCartaoCommand, CartaoDto>
{
    public async Task<CartaoDto> Handle(AtualizarCartaoCommand request, CancellationToken cancellationToken)
    {
        var cartao = await context.Cartoes.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (cartao is null || cartao.UsuarioId != currentUser.UsuarioId)
        {
            throw new NotFoundException(nameof(Cartao), request.Id);
        }

        cartao.Nome = request.Nome;
        cartao.Banco = request.Banco;
        cartao.FaturaAtual = request.FaturaAtual;
        cartao.DiaFechamento = request.DiaFechamento;
        cartao.DiaVencimento = request.DiaVencimento;

        await context.SaveChangesAsync(cancellationToken);

        return CartaoDto.DeEntidade(cartao);
    }
}
