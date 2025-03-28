using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Persistence;
using MediatR;
using System.Text.Json;

namespace EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.AddBonuses;

public class AddBonusesQueryHandler(IUnitOfWork unitOfWork, IUserContextService userContextService, ILoggingRepository loggingRepository)
    : IRequestHandler<AddBonusesCommand, List<AddBonusesDto>>
{
    public async Task<List<AddBonusesDto>> Handle(
        AddBonusesCommand request,
        CancellationToken cancellationToken)

    {
        // თანამშრომლის ნახვა PersonalNumber ით
        try
        {
	        unitOfWork.BeginTransaction();

            int userId = userContextService.GetUserId();

            var employeeExists = await unitOfWork.EmployeeRepository.GetEmployeeExistsByPersonalNumberAsync(request.PersonalNumber);
            if (!employeeExists.Item1)
            {
                throw new Exception($"თანამშრომელი პირადი ნომრით {request.PersonalNumber} არ მოიძებნა.");
            }

            var bonuses = await unitOfWork.BonusRepository.AddBonusAsync(new BonusEntity
            {
                EmployeeId = employeeExists.Item2,
                Amount = request.BonusAmount
            }, userId);

            var logEntity = new LogsEntity
            {
                TimeStamp = DateTime.UtcNow,
                UserId = userId,
                ActionType = "AddBonus",
                Request = JsonSerializer.Serialize(request),
                Response = JsonSerializer.Serialize(bonuses)

            };
            await loggingRepository.LogInformationAsync(logEntity);


	        unitOfWork.Commit();
            return bonuses;
        }

        catch (Exception ex)
        {
             unitOfWork.Rollback();
            Console.WriteLine($"[ERROR] Error occurred: {ex.Message}");

            var errorLog = new ErrorLogsEntity
            {
                TimeStamp = DateTime.UtcNow,
                UserId = userContextService.GetUserId(),
                Level = "Error",
                Message = "An error occurred while adding a Bonus.",
                Exception = ex.ToString()
            };

            await loggingRepository.LogErrorInformationAsync(errorLog);
            throw new Exception(ex.Message);
        }
    }  
}