using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.AddBonuses;

public record AddBonusesCommand(
    string PersonalNumber,
    decimal BonusAmount)
    : IRequest<List<AddBonusesDto>>;