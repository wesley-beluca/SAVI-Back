using MediatR;
using SAVi.Application.Cartoes.Common;
using SAVi.Application.Common.Interfaces;
using SAVi.Domain.Entities;

namespace SAVi.Application.Cartoes.Commands.CriarCartao;

public record CriarCartaoCommand(string Nome, string Banco, decimal FaturaAtual, int DiaFechamento, int DiaVencimento)
    : IRequest<CartaoDto>;

public class CriarCartaoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<CriarCartaoCommand, CartaoDto>
{
    public async Task<CartaoDto> Handle(CriarCartaoCommand request, CancellationToken cancellationToken)
    {
        var cartao = new Cartao
        {
            Nome = request.Nome,
            Banco = request.Banco,
            FaturaAtual = request.FaturaAtual,
            DiaFechamento = request.DiaFechamento,
            DiaVencimento = request.DiaVencimento,
            UsuarioId = currentUser.UsuarioId!.Value
        };

        context.Cartoes.Add(cartao);
        await context.SaveChangesAsync(cancellationToken);

        return CartaoDto.DeEntidade(cartao);
    }
}
