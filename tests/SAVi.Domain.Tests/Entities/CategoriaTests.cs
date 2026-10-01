using SAVi.Domain.Entities;
using SAVi.Domain.Enums;
using Xunit;

namespace SAVi.Domain.Tests.Entities;

public class CategoriaTests
{
    [Fact]
    public void NovaCategoria_SemUsuarioId_EhConsideradaPadraoDoSistema()
    {
        var categoria = new Categoria
        {
            Nome = "Alimentação",
            Tipo = TipoTransacao.Despesa,
            Icone = "utensils",
            Cor = "#F97316"
        };

        Assert.Null(categoria.UsuarioId);
    }

    [Fact]
    public void NovaCategoria_RecebeIdEDataDeCriacaoAutomaticamente()
    {
        var categoria = new Categoria
        {
            Nome = "Lazer",
            Tipo = TipoTransacao.Despesa,
            Icone = "gamepad-2",
            Cor = "#A855F7"
        };

        Assert.NotEqual(Guid.Empty, categoria.Id);
        Assert.True(categoria.CreatedAtUtc <= DateTime.UtcNow);
    }
}
