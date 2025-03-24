using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.AddBonuses;

public record AddBonusesQuery(
    string PersonalNumber,
    decimal BonusAmount)
    : IRequest<List<AddBonusesDto>>;