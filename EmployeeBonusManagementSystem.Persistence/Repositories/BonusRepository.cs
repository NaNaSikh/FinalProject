using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;
using EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.AddBonuses;
using EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.UpdateOrInsertBonusConfiguration;
using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using EmployeeBonusManagementSystem.Persistence;

public class BonusRepository() : IBonusRepository
{

	private IDbConnection _connection;
	private IDbTransaction _transaction;

	public void SetConnection(IDbConnection connection)
	{
		_connection = connection;
	}

	public void SetTransaction(IDbTransaction transaction)
	{
		_transaction = transaction;
	}
	public async Task<List<AddBonusesDto>> AddBonusAsync(BonusEntity bonus, int userId)
    {
		try
		{
			var bonusesResult = await _connection.QueryAsync<AddBonusesDto>( 
				"AddBonuses",
				new { EmployeeId = bonus.EmployeeId, BonusAmount = bonus.Amount, CreateByUserId = userId },
				commandType: CommandType.StoredProcedure,
				transaction: _transaction 
			);

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
    int? RecommendationBonusRate,
    int userId)
    {
        try
        {
			var result = await _connection.QueryAsync<UpsertBonusConfigurationDto>( 
				"UpsertBonusConfiguration",
				new
				{
					MaxBonusAmount,
					MaxBonusPercentage,
					MinBonusPercentage,
					MaxRecommendationLevel,
					RecommendationBonusRate,
					CreateByUserId = userId
				},
				commandType: CommandType.StoredProcedure,
				transaction: _transaction 
			);

			return result.ToList();
		}
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
