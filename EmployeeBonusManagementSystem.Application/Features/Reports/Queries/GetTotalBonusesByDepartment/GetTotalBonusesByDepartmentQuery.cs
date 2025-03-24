using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonusesByDepartment;

public record GetTotalBonusesByDepartmentQuery(
    DateTime StartDate,
    DateTime EndDate)
    : IRequest<List<DepartmentBonusesDto>>;