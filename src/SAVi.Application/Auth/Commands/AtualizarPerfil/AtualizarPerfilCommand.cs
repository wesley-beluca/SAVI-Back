using MediatR;
using SAVi.Application.Auth.Common;
using SAVi.Application.Common.Exceptions;
using SAVi.Application.Common.Interfaces;

namespace SAVi.Application.Auth.Commands.AtualizarPerfil;

public record AtualizarPerfilCommand(string Nome, string Email) : IRequest<AuthResponseDto>;

public class AtualizarPerfilCommandHandler(
    IIdentityService identityService, IJwtTokenService jwtTokenService, ICurrentUserService currentUser)
    : IRequestHandler<AtualizarPerfilCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(AtualizarPerfilCommand request, CancellationToken cancellationToken)
    {
        var usuarioId = currentUser.UsuarioId!.Value;

        var resultado = await identityService.AtualizarPerfilAsync(usuarioId, request.Nome, request.Email);

        if (!resultado.Sucesso)
        {
            throw new AuthenticationFailedException(resultado.Erro ?? "Não foi possível atualizar o perfil.");
        }

        var token = jwtTokenService.GerarToken(resultado.UsuarioId, resultado.Email, resultado.Nome);

        return new AuthResponseDto(
            token.Token,
            token.ExpiraEmUtc,
            new UsuarioDto(resultado.UsuarioId, resultado.Email, resultado.Nome));
    }
}
