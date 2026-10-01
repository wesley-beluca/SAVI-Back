using FluentValidation;

namespace SAVi.Application.Contas.Commands.CriarConta;

public class CriarContaCommandValidator : AbstractValidator<CriarContaCommand>
{
    public CriarContaCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Banco).NotEmpty().MaximumLength(100);
    }
}
