using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAVi.Application.Orcamentos.Commands.DefinirOrcamento;
using SAVi.Application.Orcamentos.Common;
using SAVi.Application.Orcamentos.Queries.ObterOrcamento;

namespace SAVi.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/orcamento")]
public class OrcamentoController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<OrcamentoDto>> Obter([FromQuery] int mes, [FromQuery] int ano)
        => Ok(await sender.Send(new ObterOrcamentoQuery(mes, ano)));

    [HttpPut]
    public async Task<ActionResult<OrcamentoDto>> Definir(DefinirOrcamentoCommand command)
        => Ok(await sender.Send(command));
}
