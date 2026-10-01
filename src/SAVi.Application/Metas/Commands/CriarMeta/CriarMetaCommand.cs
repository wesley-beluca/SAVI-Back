using MediatR;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Metas.Common;
using SAVi.Domain.Entities;

namespace SAVi.Application.Metas.Commands.CriarMeta;

public record CriarMetaCommand(string Nome, string Icone, string Cor, decimal ValorAtual, decimal ValorAlvo)
    : IRequest<MetaDto>;

public class CriarMetaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<CriarMetaCommand, MetaDto>
{
    public async Task<MetaDto> Handle(CriarMetaCommand request, CancellationToken cancellationToken)
    {
        var meta = new Meta
        {
            Nome = request.Nome,
            Icone = request.Icone,
            Cor = request.Cor,
            ValorAtual = request.ValorAtual,
            ValorAlvo = request.ValorAlvo,
            UsuarioId = currentUser.UsuarioId!.Value
        };

        context.Metas.Add(meta);
        await context.SaveChangesAsync(cancellationToken);

        return MetaDto.DeEntidade(meta);
    }
}
