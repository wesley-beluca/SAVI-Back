using MediatR;
using SAVi.Application.Categorias.Common;
using SAVi.Application.Common.Interfaces;
using SAVi.Domain.Entities;
using SAVi.Domain.Enums;

namespace SAVi.Application.Categorias.Commands.CriarCategoria;

public record CriarCategoriaCommand(string Nome, TipoTransacao Tipo, string Icone, string Cor)
    : IRequest<CategoriaDto>;

public class CriarCategoriaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<CriarCategoriaCommand, CategoriaDto>
{
    public async Task<CategoriaDto> Handle(CriarCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = new Categoria
        {
            Nome = request.Nome,
            Tipo = request.Tipo,
            Icone = request.Icone,
            Cor = request.Cor,
            UsuarioId = currentUser.UsuarioId
        };

        context.Categorias.Add(categoria);
        await context.SaveChangesAsync(cancellationToken);

        return CategoriaDto.DeEntidade(categoria);
    }
}
