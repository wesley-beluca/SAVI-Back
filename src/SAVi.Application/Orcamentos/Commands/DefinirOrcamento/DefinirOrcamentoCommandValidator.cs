using FluentValidation;

namespace SAVi.Application.Orcamentos.Commands.DefinirOrcamento;

public class DefinirOrcamentoCommandValidator : AbstractValidator<DefinirOrcamentoCommand>
{
    public DefinirOrcamentoCommandValidator()
    {
        RuleFor(x => x.Mes).InclusiveBetween(1, 12);
        RuleFor(x => x.Ano).InclusiveBetween(2000, 2100);
        RuleFor(x => x.RendaPrevista).GreaterThan(0);
        RuleFor(x => x.LimitePlanejado).GreaterThan(0);
    }
}
