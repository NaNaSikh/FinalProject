using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenEmployeeByBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenRecommenderByBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonusesByDepartment;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalSalariesByDepartment;
using EmployeeBonusManagementSystem.Domain.Enums;
using EmployeeBonusManagementSystem.Persistence.Services;
using EmployeeBonusManagementSystem.Persistence.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
	private readonly IReportRepository _reportRepository;
	private readonly IDateService _dateService;
	private readonly IMediator _mediator;

	public ReportController(IReportRepository reportRepository, IDateService dateService, IMediator mediator)
	{
		_reportRepository = reportRepository;
		_dateService = dateService;
		_mediator = mediator;
	}

	[HttpGet("bonuses/total")]
	public async Task<ActionResult<TotalBonusesDto>> GetTotalBonuses([FromQuery] GetTotalBonusesQuery getTotalBonusesQuery )
	{
		var result = await _mediator.Send(getTotalBonusesQuery);
	    return Ok(result);
	}

	[HttpGet("employees/top-ten-by-bonuses")]
	public async Task<ActionResult<List<EmployeeBonusesDto>>> GetTopTenEmployeesByBonuses([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
	{
		var result = await _reportRepository.GetTenEmployeeByBonusesAsync(startDate, endDate);
		return Ok(result);
	}

	[HttpGet("employees/top-ten-by-bonuses-by-time")]
	public async Task<ActionResult<List<EmployeeBonusesDto>>> GetTopTenEmployeesByBonusesByTime([FromQuery] TimeRange timeRange)
	{
		var (startDate, endDate) = _dateService.GetDateRange(timeRange);
		var result = await _reportRepository.GetTenEmployeeByBonusesAsync(startDate, endDate);
		return Ok(result);
	}


	[HttpGet("recommenders/top-ten")]
	public async Task<ActionResult<List<RecommenderBonusesDto>>> GetTopTenRecommenders([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
	{
		var result = await _reportRepository.GetTenRecommenderByBonusesAsync(startDate, endDate);
		return Ok(result);
	}

	[HttpGet("departments/salaries/total")]
	public async Task<ActionResult<List<DepartmentSalaryDto>>> GetTotalSalariesByDepartment([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
	{
		var result = await _reportRepository.GetTotalSalariesByDepartmentAsync(startDate, endDate);
		return Ok(result);
	}

	[HttpGet("departments/bonuses/total")]
	public async Task<ActionResult<List<DepartmentBonusesDto>>> GetTotalBonusesByDepartment([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
	{
		var result = await _reportRepository.GetTotalBonusesByDepartmentAsync(startDate, endDate);
		return Ok(result);
	}
}