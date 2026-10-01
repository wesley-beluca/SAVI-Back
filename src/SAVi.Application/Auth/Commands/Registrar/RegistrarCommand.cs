using MediatR;
using SAVi.Application.Auth.Common;
using SAVi.Application.Common.Exceptions;
using SAVi.Application.Common.Interfaces;

namespace SAVi.Application.Auth.Commands.Registrar;

public record RegistrarCommand(string Nome, string Email, string Senha) : IRequest<AuthResponseDto>;

public class RegistrarCommandHandler(IIdentityService identityService, IJwtTokenService jwtTokenService)
    : IRequestHandler<RegistrarCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegistrarCommand request, CancellationToken cancellationToken)
    {
        var resultado = await identityService.RegistrarAsync(request.Email, request.Senha, request.Nome);

        if (!resultado.Sucesso)
        {
            throw new AuthenticationFailedException(resultado.Erro ?? "Não foi possível concluir o registro.");
        }

        var token = jwtTokenService.GerarToken(resultado.UsuarioId, resultado.Email, resultado.Nome);

        return new AuthResponseDto(
            token.Token,
            token.ExpiraEmUtc,
            new UsuarioDto(resultado.UsuarioId, resultado.Email, resultado.Nome));
    }
}
