using FluentValidation;

namespace SAVi.Application.Cartoes.Commands.AtualizarCartao;

public class AtualizarCartaoCommandValidator : AbstractValidator<AtualizarCartaoCommand>
{
    public AtualizarCartaoCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Banco).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DiaFechamento).InclusiveBetween(1, 31);
        RuleFor(x => x.DiaVencimento).InclusiveBetween(1, 31);
    }
}
