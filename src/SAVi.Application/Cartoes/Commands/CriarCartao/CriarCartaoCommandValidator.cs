using FluentValidation;

namespace SAVi.Application.Cartoes.Commands.CriarCartao;

public class CriarCartaoCommandValidator : AbstractValidator<CriarCartaoCommand>
{
    public CriarCartaoCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Banco).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DiaFechamento).InclusiveBetween(1, 31);
        RuleFor(x => x.DiaVencimento).InclusiveBetween(1, 31);
    }
}
