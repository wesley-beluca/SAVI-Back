using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Transacoes.Common;
using SAVi.Domain.Entities;
using SAVi.Domain.Exceptions;
using ValidationException = SAVi.Application.Common.Exceptions.ValidationException;

namespace SAVi.Application.Transacoes.Commands.DefinirPagamento;

/// <summary>
/// Marca (ou desmarca) como paga uma ocorrência de uma despesa. Só despesas pagas descontam do saldo.
/// </summary>
public record DefinirPagamentoCommand(Guid Id, int Ocorrencia, bool Pago) : IRequest<TransacaoDto>;

public class DefinirPagamentoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<DefinirPagamentoCommand, TransacaoDto>
{
    public async Task<TransacaoDto> Handle(DefinirPagamentoCommand request, CancellationToken cancellationToken)
    {
        var transacao = await context.Transacoes
            .Include(t => t.Categoria)
            .Include(t => t.Pagamentos)
            .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (transacao is null || transacao.UsuarioId != currentUser.UsuarioId)
        {
            throw new NotFoundException(nameof(Transacao), request.Id);
        }

        if (!transacao.PodeSerPaga(request.Ocorrencia))
        {
            throw new ValidationException([
                new ValidationFailure(nameof(request.Ocorrencia), "Só é possível marcar como paga uma ocorrência existente de uma despesa.")
            ]);
        }

        var pagamento = transacao.PagamentoDaOcorrencia(request.Ocorrencia);

        if (request.Pago && pagamento is null)
        {
            context.PagamentosTransacoes.Add(new PagamentoTransacao { TransacaoId = transacao.Id, Ocorrencia = request.Ocorrencia });
        }
        else if (!request.Pago && pagamento is not null)
        {
            context.PagamentosTransacoes.Remove(pagamento);
        }

        await context.SaveChangesAsync(cancellationToken);

        return TransacaoDto.DeEntidade(transacao, transacao.Categoria!, request.Ocorrencia);
    }
}
