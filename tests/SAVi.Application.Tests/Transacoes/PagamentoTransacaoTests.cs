using Microsoft.EntityFrameworkCore;
using Moq;
using SAVi.Application.Common.Interfaces;
using SAVi.Application.Transacoes.Commands.AtualizarTransacao;
using SAVi.Application.Transacoes.Commands.CriarTransacao;
using SAVi.Application.Transacoes.Commands.DefinirPagamento;
using SAVi.Application.Transacoes.Queries.ListarDespesasPendentes;
using SAVi.Domain.Entities;
using SAVi.Domain.Enums;
using Xunit;
using ValidationException = SAVi.Application.Common.Exceptions.ValidationException;

namespace SAVi.Application.Tests.Transacoes;

public class PagamentoTransacaoTests
{
    private readonly Guid _usuarioId = Guid.NewGuid();
    private readonly Mock<ICurrentUserService> _currentUser = new();

    public PagamentoTransacaoTests()
    {
        _currentUser.Setup(u => u.UsuarioId).Returns(_usuarioId);
    }

    [Fact]
    public async Task CriarTransacao_ComPago_MarcaPrimeiraOcorrenciaComoPaga()
    {
        await using var context = TestApplicationDbContext.Criar();
        var categoria = await AdicionarCategoriaAsync(context);

        var handler = new CriarTransacaoCommandHandler(context, _currentUser.Object);
        var resultado = await handler.Handle(
            new CriarTransacaoCommand("Mercado", 200, new DateOnly(2026, 10, 1), TipoTransacao.Despesa, true, "Nubank", 1, 3, categoria.Id, Pago: true),
            CancellationToken.None);

        Assert.True(resultado.Pago);
        Assert.NotNull(resultado.PagoEm);
        var pagamentos = await context.PagamentosTransacoes.Where(p => p.TransacaoId == resultado.Id).ToListAsync();
        Assert.Equal(1, Assert.Single(pagamentos).Ocorrencia);
    }

    [Fact]
    public async Task CriarTransacao_ReceitaComPago_NaoRegistraPagamento()
    {
        await using var context = TestApplicationDbContext.Criar();
        var categoria = await AdicionarCategoriaAsync(context, TipoTransacao.Receita);

        var handler = new CriarTransacaoCommandHandler(context, _currentUser.Object);
        var resultado = await handler.Handle(
            new CriarTransacaoCommand("Salário", 5000, new DateOnly(2026, 10, 5), TipoTransacao.Receita, false, "Nubank", 1, 1, categoria.Id, Pago: true),
            CancellationToken.None);

        Assert.False(resultado.Pago);
        Assert.Empty(context.PagamentosTransacoes);
    }

    [Fact]
    public async Task DefinirPagamento_MarcaEDesmarcaAOcorrenciaInformada()
    {
        await using var context = TestApplicationDbContext.Criar();
        var transacao = await AdicionarDespesaAsync(context, new DateOnly(2026, 10, 10), mesesRecorrencia: 3);
        var handler = new DefinirPagamentoCommandHandler(context, _currentUser.Object);

        var paga = await handler.Handle(new DefinirPagamentoCommand(transacao.Id, 2, Pago: true), CancellationToken.None);

        Assert.True(paga.Pago);
        Assert.Equal(new DateOnly(2026, 11, 10), paga.Data);
        Assert.Equal(2, Assert.Single(context.PagamentosTransacoes).Ocorrencia);

        var desmarcada = await handler.Handle(new DefinirPagamentoCommand(transacao.Id, 2, Pago: false), CancellationToken.None);

        Assert.False(desmarcada.Pago);
        Assert.Empty(context.PagamentosTransacoes);
    }

    [Fact]
    public async Task DefinirPagamento_MarcarDuasVezes_NaoDuplicaPagamento()
    {
        await using var context = TestApplicationDbContext.Criar();
        var transacao = await AdicionarDespesaAsync(context, new DateOnly(2026, 10, 10));
        var handler = new DefinirPagamentoCommandHandler(context, _currentUser.Object);

        await handler.Handle(new DefinirPagamentoCommand(transacao.Id, 1, Pago: true), CancellationToken.None);
        await handler.Handle(new DefinirPagamentoCommand(transacao.Id, 1, Pago: true), CancellationToken.None);

        Assert.Single(context.PagamentosTransacoes);
    }

    [Fact]
    public async Task DefinirPagamento_OcorrenciaForaDaRecorrencia_LancaValidationException()
    {
        await using var context = TestApplicationDbContext.Criar();
        var transacao = await AdicionarDespesaAsync(context, new DateOnly(2026, 10, 10), mesesRecorrencia: 2);
        var handler = new DefinirPagamentoCommandHandler(context, _currentUser.Object);

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(new DefinirPagamentoCommand(transacao.Id, 3, Pago: true), CancellationToken.None));
    }

    [Fact]
    public async Task AtualizarTransacao_ReduzindoRecorrencia_RemovePagamentosQueDeixaramDeExistir()
    {
        await using var context = TestApplicationDbContext.Criar();
        var transacao = await AdicionarDespesaAsync(context, new DateOnly(2026, 10, 10), mesesRecorrencia: 4);
        context.PagamentosTransacoes.AddRange(
            new PagamentoTransacao { TransacaoId = transacao.Id, Ocorrencia = 1 },
            new PagamentoTransacao { TransacaoId = transacao.Id, Ocorrencia = 4 });
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new AtualizarTransacaoCommandHandler(context, _currentUser.Object);
        await handler.Handle(
            new AtualizarTransacaoCommand(transacao.Id, transacao.Descricao, transacao.Valor, transacao.Data, TipoTransacao.Despesa,
                true, transacao.Conta, 1, 2, transacao.CategoriaId),
            CancellationToken.None);

        Assert.Equal(1, Assert.Single(context.PagamentosTransacoes).Ocorrencia);
    }

    [Fact]
    public async Task ListarDespesasPendentes_RetornaSoOcorrenciasNaoPagasNoIntervalo()
    {
        await using var context = TestApplicationDbContext.Criar();
        var aluguel = await AdicionarDespesaAsync(context, new DateOnly(2026, 9, 20), mesesRecorrencia: 3, descricao: "Aluguel");
        var luz = await AdicionarDespesaAsync(context, new DateOnly(2026, 10, 22), descricao: "Luz");
        await AdicionarDespesaAsync(context, new DateOnly(2026, 12, 1), descricao: "Fora do intervalo");
        context.PagamentosTransacoes.Add(new PagamentoTransacao { TransacaoId = aluguel.Id, Ocorrencia = 1 });
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new ListarDespesasPendentesQueryHandler(context, _currentUser.Object);
        var pendentes = await handler.Handle(
            new ListarDespesasPendentesQuery(new DateOnly(2026, 9, 1), new DateOnly(2026, 10, 25)), CancellationToken.None);

        Assert.Collection(pendentes,
            p =>
            {
                Assert.Equal(aluguel.Id, p.Id);
                Assert.Equal(2, p.Ocorrencia);
                Assert.Equal(new DateOnly(2026, 10, 20), p.Data);
            },
            p => Assert.Equal(luz.Id, p.Id));
    }

    [Fact]
    public async Task ListarDespesasPendentes_IgnoraTransacoesDeOutrosUsuarios()
    {
        await using var context = TestApplicationDbContext.Criar();
        await AdicionarDespesaAsync(context, new DateOnly(2026, 10, 22), usuarioId: Guid.NewGuid());

        var handler = new ListarDespesasPendentesQueryHandler(context, _currentUser.Object);
        var pendentes = await handler.Handle(
            new ListarDespesasPendentesQuery(new DateOnly(2026, 10, 1), new DateOnly(2026, 10, 31)), CancellationToken.None);

        Assert.Empty(pendentes);
    }

    private static async Task<Categoria> AdicionarCategoriaAsync(
        TestApplicationDbContext context, TipoTransacao tipo = TipoTransacao.Despesa)
    {
        var categoria = new Categoria { Nome = "Moradia", Tipo = tipo, Icone = "home", Cor = "#16A34A" };
        context.Categorias.Add(categoria);
        await context.SaveChangesAsync(CancellationToken.None);
        return categoria;
    }

    private async Task<Transacao> AdicionarDespesaAsync(
        TestApplicationDbContext context, DateOnly data, int mesesRecorrencia = 1, string descricao = "Conta", Guid? usuarioId = null)
    {
        var categoria = await AdicionarCategoriaAsync(context);
        var transacao = new Transacao
        {
            Descricao = descricao,
            Valor = 100,
            Data = data,
            Tipo = TipoTransacao.Despesa,
            Recorrente = mesesRecorrencia > 1,
            MesesRecorrencia = mesesRecorrencia,
            Conta = "Nubank",
            CategoriaId = categoria.Id,
            UsuarioId = usuarioId ?? _usuarioId
        };
        context.Transacoes.Add(transacao);
        await context.SaveChangesAsync(CancellationToken.None);
        return transacao;
    }
}
