using EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.AddBonuses;
using MediatR;
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

    [HttpPost("AddBonus")]
    public async Task<ActionResult<List<AddBonusesDto>>> AddBonus([FromBody] AddBonusesCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }


}

