using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAVi.Application.Contas.Commands.AtualizarConta;
using SAVi.Application.Contas.Commands.CriarConta;
using SAVi.Application.Contas.Commands.RemoverConta;
using SAVi.Application.Contas.Common;
using SAVi.Application.Contas.Queries.ListarContas;

namespace SAVi.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/contas")]
public class ContasController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ContaDto>>> Listar()
        => Ok(await sender.Send(new ListarContasQuery()));

    [HttpPost]
    public async Task<ActionResult<ContaDto>> Criar(CriarContaCommand command)
        => Ok(await sender.Send(command));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ContaDto>> Atualizar(Guid id, AtualizarContaCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("O id da rota não confere com o id do comando.");
        }

        return Ok(await sender.Send(command));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        await sender.Send(new RemoverContaCommand(id));
        return NoContent();
    }
}
