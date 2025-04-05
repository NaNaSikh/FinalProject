using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenEmployeeByBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenRecommenderByBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonusesByDepartment;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalSalariesByDepartment;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/reports")]
public class ReportController : ControllerBase
{
	private readonly IReportRepository _reportRepository;

	public ReportController(IReportRepository reportRepository)
	{
		_reportRepository = reportRepository;
	}

	[HttpGet("bonuses/total")]
	public async Task<ActionResult<TotalBonusesDto>> GetTotalBonuses([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
	{
		var result = await _reportRepository.GetTotalBonusesAsync(startDate, endDate);
		return Ok(result);
	}

	[HttpGet("employees/top-ten-by-bonuses")]
	public async Task<ActionResult<List<EmployeeBonusesDto>>> GetTopTenEmployeesByBonuses([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
	{
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