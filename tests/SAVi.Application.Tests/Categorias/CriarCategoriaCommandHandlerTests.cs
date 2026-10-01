using Moq;
using SAVi.Application.Categorias.Commands.CriarCategoria;
using SAVi.Application.Common.Interfaces;
using SAVi.Domain.Enums;
using Xunit;

namespace SAVi.Application.Tests.Categorias;

public class CriarCategoriaCommandHandlerTests
{
    [Fact]
    public async Task Handle_DeveCriarCategoriaAssociadaAoUsuarioAtual()
    {
        await using var context = TestApplicationDbContext.Criar();

        var usuarioId = Guid.NewGuid();
        var currentUser = new Mock<ICurrentUserService>();
        currentUser.Setup(u => u.UsuarioId).Returns(usuarioId);

        var handler = new CriarCategoriaCommandHandler(context, currentUser.Object);
        var command = new CriarCategoriaCommand("Assinaturas", TipoTransacao.Despesa, "credit-card", "#7C3AED");

        var resultado = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Assinaturas", resultado.Nome);
        Assert.True(resultado.Customizada);

        var categoriaSalva = await context.Categorias.FindAsync(resultado.Id);
        Assert.NotNull(categoriaSalva);
        Assert.Equal(usuarioId, categoriaSalva!.UsuarioId);
    }
}
