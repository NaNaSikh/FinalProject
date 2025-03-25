using EmployeeBonusManagementSystem.Persistence;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.UpdateOrInsertBonusConfiguration;

public class UpsertBonusConfigurationHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpsertBonusConfigurationCommand, List<UpsertBonusConfigurationDto>>
{
    public async Task<List<UpsertBonusConfigurationDto>> Handle(
        UpsertBonusConfigurationCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.OpenAsync();
            await unitOfWork.BeginTransactionAsync();

            var result = await unitOfWork.BonusRepository.UpdateOrInsertBonusConfigurationAsync(
                request.MaxBonusAmount,
                request.MaxBonusPercentage,
                request.MinBonusPercentage,
                request.MaxRecommendationLevel,
                request.RecommendationBonusRate,
                request.CreateByUserId);

            await unitOfWork.CommitAsync();
            return result;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            throw new Exception(ex.Message);
        }
        finally
        {
            await unitOfWork.CloseAsync();
        }
    }
}
