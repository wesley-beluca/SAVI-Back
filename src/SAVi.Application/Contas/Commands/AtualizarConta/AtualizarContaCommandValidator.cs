using FluentValidation;

namespace SAVi.Application.Contas.Commands.AtualizarConta;

public class AtualizarContaCommandValidator : AbstractValidator<AtualizarContaCommand>
{
    public AtualizarContaCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Banco).NotEmpty().MaximumLength(100);
    }
}
