using EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.AddBonuses;
using EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.UpdateOrInsertBonusConfiguration;
using EmployeeBonusManagementSystem.Domain.Entities;

namespace EmployeeBonusManagementSystem.Application.Contracts.Persistence;

public interface IBonusRepository
{
    Task<List<AddBonusesDto>> AddBonusAsync(BonusEntity bonus, int userId);

    Task<List<UpsertBonusConfigurationDto>> UpdateOrInsertBonusConfigurationAsync(
    decimal? MaxBonusAmount,
    int? MaxBonusPercentage,
    int? MinBonusPercentage,
    int? MaxRecommendationLevel,
    int? RecommendationBonusRate,
    int CreateByUserId);


}

