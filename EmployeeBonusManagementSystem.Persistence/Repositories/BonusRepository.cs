using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;
using EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.AddBonuses;
using EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.UpdateOrInsertBonusConfiguration;
using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace EmployeeBonusManagementSystem.Persistence.Repositories;

public class BonusRepository(
        ISqlQueryRepository sqlQueryRepository,
        ISqlCommandRepository sqlCommandRepository,
        IConfiguration configuration)
        : IBonusRepository

{
    public async Task<List<AddBonusesDto>> AddBonusAsync(BonusEntity bonus, int userId)

    {
        try
        {
            //{
            //    var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("Id");

            //    if (userIdClaim == null)
            //    {
            //        throw new UnauthorizedAccessException("User ID not found in token.");
            //    }

            //    int userId = int.Parse(userIdClaim.Value);


            var bonusesResult = await sqlQueryRepository.LoadMultipleData<AddBonusesDto, dynamic>(
            "AddBonuses",
            new { EmployeeId = bonus.EmployeeId, BonusAmount = bonus.Amount, /*CreateByUser = userId*/ },
            configuration.GetConnectionString("DefaultConnection"),
            CommandType.StoredProcedure);

            //await unitOfWork.CommitAsync();

            return bonusesResult.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<UpsertBonusConfigurationDto>> UpdateOrInsertBonusConfigurationAsync(
    decimal? MaxBonusAmount,
    int? MaxBonusPercentage,
    int? MinBonusPercentage,
    int? MaxRecommendationLevel,
    int? RecommendationBonusRate
    /*int CreateByUserId*/)
    {
        try
        {
            var result = await sqlQueryRepository.LoadMultipleData<UpsertBonusConfigurationDto, dynamic>(
                "UpsertBonusConfiguration",
                new
                {
                    MaxBonusAmount,
                    MaxBonusPercentage,
                    MinBonusPercentage,
                    MaxRecommendationLevel,
                    RecommendationBonusRate,
                    //CreateByUserId
                },
                configuration.GetConnectionString("DefaultConnection"),
                CommandType.StoredProcedure);

            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}