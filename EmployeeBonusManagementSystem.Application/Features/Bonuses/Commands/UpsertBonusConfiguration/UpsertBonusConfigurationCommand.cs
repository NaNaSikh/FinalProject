using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.UpdateOrInsertBonusConfiguration;

public record UpsertBonusConfigurationCommand(
    decimal? MaxBonusAmount,
    int? MaxBonusPercentage,
    int? MinBonusPercentage,
    int? MaxRecommendationLevel,
    int? RecommendationBonusRate,
    int CreateByUserId)
    : IRequest<List<UpsertBonusConfigurationDto>>;


