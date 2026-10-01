using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAVi.Application.Cartoes.Commands.AtualizarCartao;
using SAVi.Application.Cartoes.Commands.CriarCartao;
using SAVi.Application.Cartoes.Commands.RemoverCartao;
using SAVi.Application.Cartoes.Common;
using SAVi.Application.Cartoes.Queries.ListarCartoes;

namespace SAVi.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/cartoes")]
public class CartoesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CartaoDto>>> Listar()
        => Ok(await sender.Send(new ListarCartoesQuery()));

    [HttpPost]
    public async Task<ActionResult<CartaoDto>> Criar(CriarCartaoCommand command)
        => Ok(await sender.Send(command));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CartaoDto>> Atualizar(Guid id, AtualizarCartaoCommand command)
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
        await sender.Send(new RemoverCartaoCommand(id));
        return NoContent();
    }
}
