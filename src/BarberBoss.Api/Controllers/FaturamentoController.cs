using BarberBoss.Application.UseCases.Faturamento.Delete;
using BarberBoss.Application.UseCases.Faturamento.GetAll;
using BarberBoss.Application.UseCases.Faturamento.GetById;
using BarberBoss.Application.UseCases.Faturamento.Register;
using BarberBoss.Application.UseCases.Faturamento.Update;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.Api.Controllers;

public class FaturamentoController : BarberBossBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredFaturamentoJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RequestFaturamentoJson request,
        [FromServices] IRegisterFaturamentoUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseFaturamentosJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllFaturamentosUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Faturamentos.Count == 0)
            return NoContent();

        return Ok(response);
    }

    [HttpGet]
    [Route("{id:long}")]
    [ProducesResponseType(typeof(ResponseFaturamentoJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] IGetFaturamentoByIdUseCase useCase, [FromRoute] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    [HttpDelete]
    [Route("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromServices] IDeleteFaturamentoUseCase useCase, [FromRoute] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }

    [HttpPut]
    [Route("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] long id, [FromBody] RequestFaturamentoJson request,
        [FromServices] IUpdateFaturamentoUseCase useCase)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }
}
