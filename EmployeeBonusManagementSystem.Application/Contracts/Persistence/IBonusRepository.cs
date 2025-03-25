using EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.AddBonuses;
using EmployeeBonusManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Contracts.Persistence;

public interface IBonusRepository
{
    public Task<List<AddBonusesDto>> AddBonusAsync(BonusEntity bonus, int userId);

}
