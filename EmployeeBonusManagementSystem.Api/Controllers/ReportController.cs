using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenEmployeeByBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenRecommenderByBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonusesByDepartment;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalSalariesByDepartment;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeBonusManagementSystem.Api.Controllers
{
    [ApiController]

    [Route("api/Reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("TotalBonuses")]
        public async Task<ActionResult<TotalBonusesDto>> GetTotalBonuses([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _reportRepository.GetTotalBonusesAsync(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("TopTenEmployeesByBonuses")]
        public async Task<ActionResult<List<EmployeeBonusesDto>>> GetTopTenEmployeesByBonuses([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _reportRepository.GetTenEmployeeByBonusesAsync(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("TopTenRecommenders")]
        public async Task<ActionResult<List<RecommenderBonusesDto>>> GetTopTenRecommenders([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _reportRepository.GetTenRecommenderByBonusesAsync(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("TotalSalariesByDepartment")]
        public async Task<ActionResult<List<DepartmentSalaryDto>>> GetTotalSalariesByDepartment([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _reportRepository.GetTotalSalariesByDepartmentAsync(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("TotalBonusesByDepartment")]
        public async Task<ActionResult<List<DepartmentBonusesDto>>> GetTotalBonusesByDepartment([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _reportRepository.GetTotalBonusesByDepartmentAsync(startDate, endDate);
            return Ok(result);
        }
    }
}
