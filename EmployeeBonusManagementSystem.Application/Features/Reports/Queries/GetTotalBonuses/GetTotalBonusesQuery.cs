using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonuses;

public record GetTotalBonusesQuery(
    DateTime StartDate,
    DateTime EndDate)
    : IRequest<TotalBonusesDto>;
