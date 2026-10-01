using SAVi.Domain.Entities;

namespace SAVi.Application.Metas.Common;

public record MetaDto(Guid Id, string Nome, string Icone, string Cor, decimal ValorAtual, decimal ValorAlvo)
{
    public static MetaDto DeEntidade(Meta meta) => new(
        meta.Id, meta.Nome, meta.Icone, meta.Cor, meta.ValorAtual, meta.ValorAlvo);
}
