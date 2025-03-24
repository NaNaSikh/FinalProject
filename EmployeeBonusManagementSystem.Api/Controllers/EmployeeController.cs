﻿using EmployeeBonusManagement.Application.Services;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Employees.Common;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries;
using EmployeeBonusManagementSystem.Application.Features.Employees;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.AddEmployee;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.ChaingePassword;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.Login;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeBonus;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeRecomendator;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeSalary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeBonusManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/Employees")]
    public class EmployeeController : ControllerBase
    {
	    private readonly IMediator _mediator;

	    public EmployeeController(IMediator mediator)
	    {
		    _mediator = mediator;
	    }

	    [HttpGet]
	    [Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllEmployees()
	    {
		    var employees = await _mediator.Send(new GetAllEmployeesQuery());
		    return Ok(employees);
	    }

		[HttpPost("Employee")]
	    [Authorize(Roles = "Admin")] 
		public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
	    {
		    var result = await _mediator.Send(new AddEmployeeCommand(employeeDto));

		    if (result)
		    {
			    return Ok(new { message = "Employee added successfully" });
		    }

		    return BadRequest(new { message = "Failed to add employee" });
	    }

		   
	    [Authorize("User")]
		[HttpGet("Bonus")]
	    public async Task<ActionResult<List<GetEmployeeBonusDto>>> GetEmoloyeeBonus()
	    {
		    var result = await _mediator.Send(new GetEmployeeBonusQuery());
		    return Ok(result);
	    }


		[Authorize("User")]
	    [HttpGet("Salary")]
	    public async Task<ActionResult<List<GetEmployeeBonusDto>>> GetEmoloyeeSalary()
	    {
			var result = await _mediator.Send(new GetEmployeeSalaryQuery());
			return Ok(result);
		}

	    [Authorize("User")]
	    [HttpGet("Recommender")]
	    public async Task<ActionResult<List<GetEmployeeRecommenderDto>>> GetEmoloyeeRecommender()
	    {
		    var result = await _mediator.Send(new GetEmployeeRecommenderQuery());
		    return Ok(result);
	    }

		[Authorize("User")]
		[HttpPost("Password")]
		public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordCommand command)
		{
			var result = await _mediator.Send(command);

			if (result.Success)
				return Ok(result);

			return BadRequest(result);
		}

	}
}
