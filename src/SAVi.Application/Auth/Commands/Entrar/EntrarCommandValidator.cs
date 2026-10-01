using FluentValidation;

namespace SAVi.Application.Auth.Commands.Entrar;

public class EntrarCommandValidator : AbstractValidator<EntrarCommand>
{
    public EntrarCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Senha).NotEmpty();
    }
}
