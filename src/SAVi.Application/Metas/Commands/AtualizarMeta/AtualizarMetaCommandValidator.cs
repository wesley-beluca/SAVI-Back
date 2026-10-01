using FluentValidation;

namespace SAVi.Application.Metas.Commands.AtualizarMeta;

public class AtualizarMetaCommandValidator : AbstractValidator<AtualizarMetaCommand>
{
    public AtualizarMetaCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Icone).NotEmpty();
        RuleFor(x => x.Cor).NotEmpty().Matches("^#[0-9A-Fa-f]{6}$")
            .WithMessage("Cor precisa estar no formato hexadecimal, ex: #64748B.");
        RuleFor(x => x.ValorAtual).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ValorAlvo).GreaterThan(0);
    }
}
