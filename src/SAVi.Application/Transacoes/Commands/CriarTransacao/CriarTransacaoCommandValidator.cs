using FluentValidation;

namespace SAVi.Application.Transacoes.Commands.CriarTransacao;

public class CriarTransacaoCommandValidator : AbstractValidator<CriarTransacaoCommand>
{
    public CriarTransacaoCommandValidator()
    {
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Valor).GreaterThan(0);
        RuleFor(x => x.Conta).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Parcelas).GreaterThanOrEqualTo(1);
        RuleFor(x => x.MesesRecorrencia).InclusiveBetween(1, 120);
        RuleFor(x => x.CategoriaId).NotEmpty();
    }
}
