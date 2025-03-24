using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalSalariesByDepartment;

public record GetTotalSalariesByDepartmentQuery(
    DateTime StartDate,
    DateTime EndDate)
    : IRequest<List<DepartmentSalaryDto>>;

