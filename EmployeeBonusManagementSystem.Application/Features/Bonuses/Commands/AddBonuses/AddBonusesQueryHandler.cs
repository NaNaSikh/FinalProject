using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Persistence;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.AddBonuses;

public class AddBonusesQueryHandler( IUnitOfWork unitOfWork) : IRequestHandler<AddBonusesQuery, List<AddBonusesDto>>
{
    public async Task<List<AddBonusesDto>> Handle(
        AddBonusesQuery request,
        CancellationToken cancellationToken)

    {
        // თანამშრომლის ნახვა PersonalNumber ით
        try
        {
            await unitOfWork.OpenAsync();
            await unitOfWork.BeginTransactionAsync();

            var employeeExists = await unitOfWork.EmployeeRepository.GetEmployeeExistsByPersonalNumberAsync(request.PersonalNumber);
            if (!employeeExists.Item1)
            {
                throw new Exception($"თანამშრომელი პირადი ნომრით {request.PersonalNumber} არ მოიძებნა.");
            }


            var bonuses = await unitOfWork.BonusRepository.AddBonusAsync(new BonusEntity
            {
                EmployeeId = employeeExists.Item2,
                Amount = request.BonusAmount
            });

            await unitOfWork.CommitAsync();
            return bonuses;
        }

        //    var reason = $"{DateTime.UtcNow:MMMM} თვის ბონუსის ჩარიცხვა";

        //    var mainBonus = new BonusEntity
        //    {
        //        EmployeeId = employeeExists.Item2,
        //        Amount = request.BonusAmount,
        //        Reason = reason,
        //        CreateDate = DateTime.UtcNow,
        //        IsRecommenderBonus = false,
        //        RecommendationLevel = 0,
        //        //CreateByUserId = adminUserId, ეს ჯერ არ მაქვს
        //    };
        //    //int adminUserId = currentUserService.GetUserId(); ეს JWT

        //    await unitOfWork.BonusRepository.AddBonusAsync(mainBonus);
        //    await unitOfWork.CommitAsync();

        //    var bonuses = new List<AddBonusesDto>
        //    {
        //        new()
        //        {
        //            EmployeeId = mainBonus.EmployeeId,
        //            Amount = mainBonus.Amount,
        //            Reason = mainBonus.Reason,
        //            CreateDate = mainBonus.CreateDate,
        //            RecommendationLevel = mainBonus.RecommendationLevel,
        //            IsRecommenderBonus = mainBonus.IsRecommenderBonus
        //        }
        //    };

        //    return bonuses;
        //}
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