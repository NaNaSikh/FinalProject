﻿using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Persistence;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.UpdateOrInsertBonusConfiguration;

public class UpsertBonusConfigurationHandler(
    IUnitOfWork unitOfWork,
    IUserContextService userContextService)
    : IRequestHandler<UpsertBonusConfigurationCommand, List<UpsertBonusConfigurationDto>>
{
    public async Task<List<UpsertBonusConfigurationDto>> Handle(
        UpsertBonusConfigurationCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
           // await unitOfWork.OpenAsync();
	        unitOfWork.BeginTransaction();

            var userId = userContextService.GetUserId();


            var result = await unitOfWork.BonusRepository.UpdateOrInsertBonusConfigurationAsync(
                request.MaxBonusAmount,
                request.MaxBonusPercentage,
                request.MinBonusPercentage,
                request.MaxRecommendationLevel,
                request.RecommendationBonusRate,
                userId);

	        unitOfWork.Commit();
            return result;
        }
        catch (Exception ex)
        {
	        unitOfWork.Rollback();
            throw new Exception(ex.Message);
        }
    }
}
