using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Orcamentos.Common;
using SAVi.Application.Transacoes.Common;
using SAVi.Domain.Entities;

namespace SAVi.Application.Orcamentos.Commands.DefinirOrcamento;

public record DefinirOrcamentoCommand(int Mes, int Ano, decimal RendaPrevista, decimal LimitePlanejado)
    : IRequest<OrcamentoDto>;

public class DefinirOrcamentoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<DefinirOrcamentoCommand, OrcamentoDto>
{
    public async Task<OrcamentoDto> Handle(DefinirOrcamentoCommand request, CancellationToken cancellationToken)
    {
        var usuarioId = currentUser.UsuarioId!.Value;

        var orcamento = await context.Orcamentos
            .SingleOrDefaultAsync(
                o => o.UsuarioId == usuarioId && o.Mes == request.Mes && o.Ano == request.Ano,
                cancellationToken);

        if (orcamento is null)
        {
            orcamento = new Orcamento
            {
                Mes = request.Mes,
                Ano = request.Ano,
                RendaPrevista = request.RendaPrevista,
                LimitePlanejado = request.LimitePlanejado,
                UsuarioId = usuarioId
            };
            context.Orcamentos.Add(orcamento);
        }
        else
        {
            orcamento.RendaPrevista = request.RendaPrevista;
            orcamento.LimitePlanejado = request.LimitePlanejado;
        }

        await context.SaveChangesAsync(cancellationToken);

        var utilizado = await context.Transacoes
            .SomarDespesasDoMesAsync(usuarioId, request.Ano, request.Mes, cancellationToken);

        return new OrcamentoDto(request.Mes, request.Ano, orcamento.RendaPrevista, orcamento.LimitePlanejado, utilizado);
    }
}
