using SAVi.Domain.Entities;
using SAVi.Domain.Enums;
using Xunit;

namespace SAVi.Domain.Tests.Entities;

public class TransacaoTests
{
    [Fact]
    public void OcorrenciasEntre_RetornaApenasOcorrenciasDentroDoIntervalo()
    {
        var transacao = CriarTransacao(TipoTransacao.Despesa, new DateOnly(2026, 1, 10), mesesRecorrencia: 6);

        var ocorrencias = transacao.OcorrenciasEntre(new DateOnly(2026, 3, 1), new DateOnly(2026, 4, 10));

        Assert.Equal([3, 4], ocorrencias);
    }

    [Fact]
    public void OcorrenciasEntre_ConsideraLimitesInclusivos()
    {
        var transacao = CriarTransacao(TipoTransacao.Despesa, new DateOnly(2026, 5, 15));

        Assert.Equal([1], transacao.OcorrenciasEntre(new DateOnly(2026, 5, 15), new DateOnly(2026, 5, 15)));
        Assert.Empty(transacao.OcorrenciasEntre(new DateOnly(2026, 5, 16), new DateOnly(2026, 6, 30)));
    }

    [Theory]
    [InlineData(TipoTransacao.Despesa, 1, true)]
    [InlineData(TipoTransacao.Despesa, 3, true)]
    [InlineData(TipoTransacao.Despesa, 4, false)]
    [InlineData(TipoTransacao.Despesa, 0, false)]
    [InlineData(TipoTransacao.Receita, 1, false)]
    public void PodeSerPaga_SoAceitaOcorrenciasExistentesDeDespesas(TipoTransacao tipo, int ocorrencia, bool esperado)
    {
        var transacao = CriarTransacao(tipo, new DateOnly(2026, 1, 1), mesesRecorrencia: 3);

        Assert.Equal(esperado, transacao.PodeSerPaga(ocorrencia));
    }

    private static Transacao CriarTransacao(TipoTransacao tipo, DateOnly data, int mesesRecorrencia = 1) => new()
    {
        Descricao = "Conta de luz",
        Valor = 150,
        Data = data,
        Tipo = tipo,
        Recorrente = mesesRecorrencia > 1,
        MesesRecorrencia = mesesRecorrencia,
        Conta = "Nubank",
        CategoriaId = Guid.NewGuid(),
        UsuarioId = Guid.NewGuid()
    };
}
