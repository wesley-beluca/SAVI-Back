using FluentValidation;

namespace SAVi.Application.Auth.Commands.Registrar;

public class RegistrarCommandValidator : AbstractValidator<RegistrarCommand>
{
    public RegistrarCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Senha).NotEmpty().MinimumLength(8)
            .WithMessage("A senha precisa ter pelo menos 8 caracteres.");
    }
}
