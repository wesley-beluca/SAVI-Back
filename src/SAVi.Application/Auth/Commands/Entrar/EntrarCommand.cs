using MediatR;
using SAVi.Application.Auth.Common;
using SAVi.Application.Common.Exceptions;
using SAVi.Application.Common.Interfaces;

namespace SAVi.Application.Auth.Commands.Entrar;

public record EntrarCommand(string Email, string Senha) : IRequest<AuthResponseDto>;

public class EntrarCommandHandler(IIdentityService identityService, IJwtTokenService jwtTokenService)
    : IRequestHandler<EntrarCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(EntrarCommand request, CancellationToken cancellationToken)
    {
        var resultado = await identityService.EntrarAsync(request.Email, request.Senha);

        if (!resultado.Sucesso)
        {
            throw new AuthenticationFailedException(resultado.Erro ?? "Email ou senha inválidos.");
        }

        var token = jwtTokenService.GerarToken(resultado.UsuarioId, resultado.Email, resultado.Nome);

        return new AuthResponseDto(
            token.Token,
            token.ExpiraEmUtc,
            new UsuarioDto(resultado.UsuarioId, resultado.Email, resultado.Nome));
    }
}
