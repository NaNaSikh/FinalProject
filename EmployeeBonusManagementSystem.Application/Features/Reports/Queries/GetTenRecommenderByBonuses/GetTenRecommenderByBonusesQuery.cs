using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenRecommenderByBonuses;

public record GetTenRecommenderByBonusesQuery(
    DateTime StartDate,
    DateTime EndDate)
    : IRequest<List<RecommenderBonusesDto>>;
