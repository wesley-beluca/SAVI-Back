using MediatR;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Transacoes.Common;
using SAVi.Domain.Entities;
using SAVi.Domain.Enums;
using SAVi.Domain.Exceptions;

namespace SAVi.Application.Transacoes.Commands.CriarTransacao;

public record CriarTransacaoCommand(
    string Descricao,
    decimal Valor,
    DateOnly Data,
    TipoTransacao Tipo,
    bool Recorrente,
    string Conta,
    int Parcelas,
    int MesesRecorrencia,
    Guid CategoriaId) : IRequest<TransacaoDto>;

public class CriarTransacaoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<CriarTransacaoCommand, TransacaoDto>
{
    public async Task<TransacaoDto> Handle(CriarTransacaoCommand request, CancellationToken cancellationToken)
    {
        var usuarioId = currentUser.UsuarioId!.Value;

        var categoria = await context.Categorias.FindAsync([request.CategoriaId], cancellationToken);
        if (categoria is null)
        {
            throw new NotFoundException(nameof(Categoria), request.CategoriaId);
        }

        var transacao = new Transacao
        {
            Descricao = request.Descricao,
            Valor = request.Valor,
            Data = request.Data,
            Tipo = request.Tipo,
            Recorrente = request.Recorrente,
            Conta = request.Conta,
            Parcelas = request.Parcelas,
            MesesRecorrencia = request.Recorrente ? request.MesesRecorrencia : 1,
            CategoriaId = request.CategoriaId,
            UsuarioId = usuarioId
        };

        context.Transacoes.Add(transacao);
        await context.SaveChangesAsync(cancellationToken);

        return TransacaoDto.DeEntidade(transacao, categoria);
    }
}
