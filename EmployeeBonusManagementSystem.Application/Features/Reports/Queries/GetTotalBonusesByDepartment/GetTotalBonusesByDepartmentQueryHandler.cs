using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonusesByDepartment;

internal class GetTotalBonusesByDepartmentQueryHandler(
  IReportRepository reportRepository)
: IRequestHandler<GetTotalBonusesByDepartmentQuery, List<DepartmentBonusesDto>>
{
    public async Task<List<DepartmentBonusesDto>> Handle(
    GetTotalBonusesByDepartmentQuery request,
    CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetTotalBonusesByDepartmentAsync(request.StartDate, request.EndDate);
        return result;
    }
}
