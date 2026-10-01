using SAVi.Domain.Entities;
using SAVi.Domain.Enums;

namespace SAVi.Infrastructure.Persistence.Seed;

// Ids fixos de propósito: HasData do EF Core exige chaves estáveis entre migrations.
public static class CategoriasPadraoSeed
{
    public static readonly Categoria[] Categorias =
    [
        Nova("11111111-1111-1111-1111-000000000001", "Salário", TipoTransacao.Receita, "wallet", "#16A34A"),
        Nova("11111111-1111-1111-1111-000000000002", "Outras receitas", TipoTransacao.Receita, "plus-circle", "#22C55E"),
        Nova("11111111-1111-1111-1111-000000000003", "Alimentação", TipoTransacao.Despesa, "utensils", "#F97316"),
        Nova("11111111-1111-1111-1111-000000000004", "Moradia", TipoTransacao.Despesa, "home", "#0EA5E9"),
        Nova("11111111-1111-1111-1111-000000000005", "Transporte", TipoTransacao.Despesa, "car", "#6366F1"),
        Nova("11111111-1111-1111-1111-000000000006", "Saúde", TipoTransacao.Despesa, "heart-pulse", "#EF4444"),
        Nova("11111111-1111-1111-1111-000000000007", "Lazer", TipoTransacao.Despesa, "gamepad-2", "#A855F7"),
        Nova("11111111-1111-1111-1111-000000000008", "Educação", TipoTransacao.Despesa, "book-open", "#0D9488"),
        Nova("11111111-1111-1111-1111-000000000009", "Outras despesas", TipoTransacao.Despesa, "tag", "#64748B"),
    ];

    private static Categoria Nova(string id, string nome, TipoTransacao tipo, string icone, string cor) => new()
    {
        Id = Guid.Parse(id),
        Nome = nome,
        Tipo = tipo,
        Icone = icone,
        Cor = cor,
        UsuarioId = null,
        CreatedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    };
}
