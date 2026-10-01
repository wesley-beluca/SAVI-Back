using FluentValidation;

namespace SAVi.Application.Auth.Commands.EntrarComGoogle;

public class EntrarComGoogleCommandValidator : AbstractValidator<EntrarComGoogleCommand>
{
    public EntrarComGoogleCommandValidator()
    {
        RuleFor(x => x.IdToken).NotEmpty();
    }
}
