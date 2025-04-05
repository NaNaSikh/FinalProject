using EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.AddBonuses;
using EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.UpdateOrInsertBonusConfiguration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeBonusManagementSystem.Api.Controllers;

[ApiController]
[Route("api/Bonus")]
public class BonusController : ControllerBase
{
    private readonly IMediator _mediator;

    public BonusController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [Authorize(Roles = "Admin")]
    [HttpPost("add")]
    public async Task<ActionResult<List<AddBonusesDto>>> AddBonus([FromQuery] AddBonusesCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost("configuration")]
    public async Task<ActionResult<List<UpsertBonusConfigurationDto>>> UpsertBonusConfiguration([FromQuery] UpsertBonusConfigurationCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }


}

