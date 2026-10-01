using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAVi.Application.Metas.Commands.AtualizarMeta;
using SAVi.Application.Metas.Commands.CriarMeta;
using SAVi.Application.Metas.Commands.RemoverMeta;
using SAVi.Application.Metas.Common;
using SAVi.Application.Metas.Queries.ListarMetas;

namespace SAVi.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/metas")]
public class MetasController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MetaDto>>> Listar()
        => Ok(await sender.Send(new ListarMetasQuery()));

    [HttpPost]
    public async Task<ActionResult<MetaDto>> Criar(CriarMetaCommand command)
        => Ok(await sender.Send(command));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MetaDto>> Atualizar(Guid id, AtualizarMetaCommand command)
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
        await sender.Send(new RemoverMetaCommand(id));
        return NoContent();
    }
}
