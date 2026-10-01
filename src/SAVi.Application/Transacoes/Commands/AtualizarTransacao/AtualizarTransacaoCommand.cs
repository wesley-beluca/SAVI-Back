using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Transacoes.Common;
using SAVi.Domain.Entities;
using SAVi.Domain.Enums;
using SAVi.Domain.Exceptions;

namespace SAVi.Application.Transacoes.Commands.AtualizarTransacao;

public record AtualizarTransacaoCommand(
    Guid Id,
    string Descricao,
    decimal Valor,
    DateOnly Data,
    TipoTransacao Tipo,
    bool Recorrente,
    string Conta,
    int Parcelas,
    int MesesRecorrencia,
    Guid CategoriaId) : IRequest<TransacaoDto>;

public class AtualizarTransacaoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<AtualizarTransacaoCommand, TransacaoDto>
{
    public async Task<TransacaoDto> Handle(AtualizarTransacaoCommand request, CancellationToken cancellationToken)
    {
        var transacao = await context.Transacoes
            .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (transacao is null || transacao.UsuarioId != currentUser.UsuarioId)
        {
            throw new NotFoundException(nameof(Transacao), request.Id);
        }

        var categoria = await context.Categorias.FindAsync([request.CategoriaId], cancellationToken);
        if (categoria is null)
        {
            throw new NotFoundException(nameof(Categoria), request.CategoriaId);
        }

        transacao.Descricao = request.Descricao;
        transacao.Valor = request.Valor;
        transacao.Data = request.Data;
        transacao.Tipo = request.Tipo;
        transacao.Recorrente = request.Recorrente;
        transacao.Conta = request.Conta;
        transacao.Parcelas = request.Parcelas;
        transacao.MesesRecorrencia = request.Recorrente ? request.MesesRecorrencia : 1;
        transacao.CategoriaId = request.CategoriaId;

        await context.SaveChangesAsync(cancellationToken);

        return TransacaoDto.DeEntidade(transacao, categoria);
    }
}
