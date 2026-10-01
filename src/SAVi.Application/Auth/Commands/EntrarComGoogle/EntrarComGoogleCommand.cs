using MediatR;
using SAVi.Application.Auth.Common;
using SAVi.Application.Common.Exceptions;
using SAVi.Application.Common.Interfaces;

namespace SAVi.Application.Auth.Commands.EntrarComGoogle;

public record EntrarComGoogleCommand(string IdToken) : IRequest<AuthResponseDto>;

public class EntrarComGoogleCommandHandler(
    IGoogleTokenValidator googleTokenValidator,
    IIdentityService identityService,
    IJwtTokenService jwtTokenService)
    : IRequestHandler<EntrarComGoogleCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(EntrarComGoogleCommand request, CancellationToken cancellationToken)
    {
        var googleUsuario = await googleTokenValidator.ValidarAsync(request.IdToken);

        if (googleUsuario is null)
        {
            throw new AuthenticationFailedException("Token do Google inválido ou expirado.");
        }

        var resultado = await identityService.EntrarOuRegistrarComGoogleAsync(
            googleUsuario.Subject, googleUsuario.Email, googleUsuario.Nome);

        if (!resultado.Sucesso)
        {
            throw new AuthenticationFailedException(resultado.Erro ?? "Não foi possível entrar com o Google.");
        }

        var token = jwtTokenService.GerarToken(resultado.UsuarioId, resultado.Email, resultado.Nome);

        return new AuthResponseDto(
            token.Token,
            token.ExpiraEmUtc,
            new UsuarioDto(resultado.UsuarioId, resultado.Email, resultado.Nome));
    }
}
