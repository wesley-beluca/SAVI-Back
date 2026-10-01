using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAVi.Application.Transacoes.Commands.AtualizarTransacao;
using SAVi.Application.Transacoes.Commands.CriarTransacao;
using SAVi.Application.Transacoes.Commands.RemoverTransacao;
using SAVi.Application.Transacoes.Common;
using SAVi.Application.Transacoes.Queries.ListarTransacoes;
using SAVi.Application.Transacoes.Queries.ObterEvolucaoMensal;

namespace SAVi.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/transacoes")]
public class TransacoesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TransacaoDto>>> Listar([FromQuery] int mes, [FromQuery] int ano)
        => Ok(await sender.Send(new ListarTransacoesQuery(mes, ano)));

    [HttpGet("evolucao-mensal")]
    public async Task<ActionResult<IReadOnlyList<PontoEvolucaoMensalDto>>> ObterEvolucaoMensal([FromQuery] int meses = 12)
        => Ok(await sender.Send(new ObterEvolucaoMensalQuery(meses)));

    [HttpPost]
    public async Task<ActionResult<TransacaoDto>> Criar(CriarTransacaoCommand command)
        => Ok(await sender.Send(command));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TransacaoDto>> Atualizar(Guid id, AtualizarTransacaoCommand command)
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
        await sender.Send(new RemoverTransacaoCommand(id));
        return NoContent();
    }
}
