using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenRecommenderByBonuses;

internal class GetTenRecommenderByBonusesQueryHandler(
  IReportRepository reportRepository)
: IRequestHandler<GetTenRecommenderByBonusesQuery, List<RecommenderBonusesDto>>
{
    public async Task<List<RecommenderBonusesDto>> Handle(
    GetTenRecommenderByBonusesQuery request,
    CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetTenRecommenderByBonusesAsync(request.StartDate, request.EndDate);
        return result;
    }
}