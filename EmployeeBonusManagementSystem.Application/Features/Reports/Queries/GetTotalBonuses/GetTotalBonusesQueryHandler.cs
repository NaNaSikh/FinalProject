using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonuses;

internal class GetTotalBonusesQueryHandler(
    IReportRepository reportRepository)
    : IRequestHandler<GetTotalBonusesQuery, TotalBonusesDto>
{
    public async Task<TotalBonusesDto> Handle(
        GetTotalBonusesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetTotalBonusesAsync(request.StartDate, request.EndDate);
        return result;
    }
}