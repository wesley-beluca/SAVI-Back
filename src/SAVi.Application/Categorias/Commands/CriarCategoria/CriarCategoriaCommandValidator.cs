using FluentValidation;

namespace SAVi.Application.Categorias.Commands.CriarCategoria;

public class CriarCategoriaCommandValidator : AbstractValidator<CriarCategoriaCommand>
{
    public CriarCategoriaCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Icone).NotEmpty();
        RuleFor(x => x.Cor).NotEmpty().Matches("^#[0-9A-Fa-f]{6}$")
            .WithMessage("Cor precisa estar no formato hexadecimal, ex: #64748B.");
    }
}
