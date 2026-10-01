using MediatR;
using Microsoft.EntityFrameworkCore;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Metas.Common;
using SAVi.Domain.Entities;
using SAVi.Domain.Exceptions;

namespace SAVi.Application.Metas.Commands.AtualizarMeta;

public record AtualizarMetaCommand(Guid Id, string Nome, string Icone, string Cor, decimal ValorAtual, decimal ValorAlvo)
    : IRequest<MetaDto>;

public class AtualizarMetaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<AtualizarMetaCommand, MetaDto>
{
    public async Task<MetaDto> Handle(AtualizarMetaCommand request, CancellationToken cancellationToken)
    {
        var meta = await context.Metas.SingleOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (meta is null || meta.UsuarioId != currentUser.UsuarioId)
        {
            throw new NotFoundException(nameof(Meta), request.Id);
        }

        meta.Nome = request.Nome;
        meta.Icone = request.Icone;
        meta.Cor = request.Cor;
        meta.ValorAtual = request.ValorAtual;
        meta.ValorAlvo = request.ValorAlvo;

        await context.SaveChangesAsync(cancellationToken);

        return MetaDto.DeEntidade(meta);
    }
}
