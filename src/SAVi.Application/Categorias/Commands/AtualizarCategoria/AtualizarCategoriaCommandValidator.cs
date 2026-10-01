using FluentValidation;

namespace SAVi.Application.Categorias.Commands.AtualizarCategoria;

public class AtualizarCategoriaCommandValidator : AbstractValidator<AtualizarCategoriaCommand>
{
    public AtualizarCategoriaCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Icone).NotEmpty();
        RuleFor(x => x.Cor).NotEmpty().Matches("^#[0-9A-Fa-f]{6}$");
    }
}
