using EmployeeBonusManagementSystem.Application.Contracts.Persistence;

using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenEmployeeByBonuses;

internal class GetTenEmployeeByBonusesQueryHandler( IReportRepository reportRepository): IRequestHandler<GetTenEmployeeByBonusesQuery, List<EmployeeBonusesDto>>
{
    public async Task<List<EmployeeBonusesDto>> Handle(
        GetTenEmployeeByBonusesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetTenEmployeeByBonusesAsync(request.StartDate, request.EndDate);
        return result;
    }
}

