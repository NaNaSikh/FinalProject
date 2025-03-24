using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenEmployeeByBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenRecommenderByBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonusesByDepartment;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalSalariesByDepartment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Contracts.Persistence;
public interface IReportRepository
{
    Task<TotalBonusesDto> GetTotalBonusesAsync(DateTime startDate, DateTime endDate);
    Task<List<EmployeeBonusesDto>> GetTenEmployeeByBonusesAsync(DateTime startDate, DateTime endDate);
    Task<List<RecommenderBonusesDto>> GetTenRecommenderByBonusesAsync(DateTime startDate, DateTime endDate);
    Task<List<DepartmentSalaryDto>> GetTotalSalariesByDepartmentAsync(DateTime startDate, DateTime endDate);
    Task<List<DepartmentBonusesDto>> GetTotalBonusesByDepartmentAsync(DateTime startDate, DateTime endDate);
}