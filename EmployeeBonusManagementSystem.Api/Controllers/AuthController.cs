﻿using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.RefreshToken;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeBonusManagementSystem.Api.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

	    private readonly IMediator _mediator;

	    public AuthController(IMediator mediator)
	    {
		    _mediator = mediator;
	    }


		[HttpPost("login")]
	    public async Task<IActionResult> Login([FromQuery] LoginDto loginDto)
	    {
		    var result = await _mediator.Send(new LoginCommand(loginDto));

		    
			return Ok(result);

		   // return Unauthorized(new { message = "Invalid credentials" });
	    }

	    [HttpPost("token/refresh")]
	    public async Task<IActionResult> RefreshToken([FromQuery] RefreshTokenRequestDto refreshTokenRequest)
	    {
		    var result = await _mediator.Send(new RefreshTokenCommand(refreshTokenRequest.RefreshToken));

		    if (result == null)
		    {
			    return Unauthorized("Invalid refresh token.");
		    }

		    return Ok(result);
	    }


	}
}
