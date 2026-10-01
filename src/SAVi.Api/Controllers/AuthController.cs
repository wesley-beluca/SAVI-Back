using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAVi.Application.Auth.Commands.AtualizarPerfil;
using SAVi.Application.Auth.Commands.Entrar;
using SAVi.Application.Auth.Commands.EntrarComGoogle;
using SAVi.Application.Auth.Commands.Registrar;
using SAVi.Application.Auth.Common;

namespace SAVi.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("registrar")]
    public async Task<ActionResult<AuthResponseDto>> Registrar(RegistrarCommand command)
        => Ok(await sender.Send(command));

    [HttpPost("entrar")]
    public async Task<ActionResult<AuthResponseDto>> Entrar(EntrarCommand command)
        => Ok(await sender.Send(command));

    [HttpPost("google")]
    public async Task<ActionResult<AuthResponseDto>> EntrarComGoogle(EntrarComGoogleCommand command)
        => Ok(await sender.Send(command));

    [HttpPut("perfil")]
    [Authorize]
    public async Task<ActionResult<AuthResponseDto>> AtualizarPerfil(AtualizarPerfilCommand command)
        => Ok(await sender.Send(command));
}
