using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalSalariesByDepartment;

internal class GetTotalSalariesByDepartmentQueryHandler(
  IReportRepository reportRepository)
: IRequestHandler<GetTotalSalariesByDepartmentQuery, List<DepartmentSalaryDto>>
{
    public async Task<List<DepartmentSalaryDto>> Handle(
    GetTotalSalariesByDepartmentQuery request,
    CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetTotalSalariesByDepartmentAsync(request.StartDate, request.EndDate);
        return result;
    }
}
