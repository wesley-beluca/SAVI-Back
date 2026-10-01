using FluentValidation;

namespace SAVi.Application.Auth.Commands.AtualizarPerfil;

public class AtualizarPerfilCommandValidator : AbstractValidator<AtualizarPerfilCommand>
{
    public AtualizarPerfilCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
    }
}
