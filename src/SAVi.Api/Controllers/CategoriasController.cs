using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAVi.Application.Categorias.Commands.AtualizarCategoria;
using SAVi.Application.Categorias.Commands.CriarCategoria;
using SAVi.Application.Categorias.Commands.RemoverCategoria;
using SAVi.Application.Categorias.Common;
using SAVi.Application.Categorias.Queries.ListarCategorias;

namespace SAVi.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/categorias")]
public class CategoriasController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoriaDto>>> Listar()
        => Ok(await sender.Send(new ListarCategoriasQuery()));

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Criar(CriarCategoriaCommand command)
        => Ok(await sender.Send(command));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CategoriaDto>> Atualizar(Guid id, AtualizarCategoriaCommand command)
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
        await sender.Send(new RemoverCategoriaCommand(id));
        return NoContent();
    }
}
