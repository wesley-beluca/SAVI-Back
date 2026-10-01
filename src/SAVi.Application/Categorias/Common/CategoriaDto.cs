using SAVi.Domain.Entities;
using SAVi.Domain.Enums;

namespace SAVi.Application.Categorias.Common;

public record CategoriaDto(Guid Id, string Nome, TipoTransacao Tipo, string Icone, string Cor, bool Customizada)
{
    public static CategoriaDto DeEntidade(Categoria categoria) => new(
        categoria.Id,
        categoria.Nome,
        categoria.Tipo,
        categoria.Icone,
        categoria.Cor,
        categoria.UsuarioId is not null);
}
