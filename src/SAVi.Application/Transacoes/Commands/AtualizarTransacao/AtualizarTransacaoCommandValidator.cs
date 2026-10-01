using FluentValidation;

namespace SAVi.Application.Transacoes.Commands.AtualizarTransacao;

public class AtualizarTransacaoCommandValidator : AbstractValidator<AtualizarTransacaoCommand>
{
    public AtualizarTransacaoCommandValidator()
    {
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Valor).GreaterThan(0);
        RuleFor(x => x.Conta).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Parcelas).GreaterThanOrEqualTo(1);
        RuleFor(x => x.MesesRecorrencia).InclusiveBetween(1, 120);
        RuleFor(x => x.CategoriaId).NotEmpty();
    }
}
