using EmployeeBonusManagementSystem.Domain.Entities;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace EmployeeBonusManagementSystem.Application.Contracts.Persistence;

public interface IEmployeeRepository
{
	Task AddEmployeeAsync(EmployeeEntity employee, string role);
	Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync();
	Task<EmployeeEntity> GetByEmailAsync(string email);
	Task<List<string>> GetUserRolesAsync(int employeeId);
	Task<IEnumerable<EmployeeEntity>> GetEmployeeSalary(int Id);
	Task<IEnumerable<BonusEntity>> GetEmployeeBonus(int Id);
	Task<IEnumerable<EmployeeEntity>> GetEmployeeRecommender(int Id);
    Task<(bool, int)> GetEmployeeExistsByPersonalNumberAsync(string personalNumber);
	Task UpdateEmployeePasswordByIdAsync(int Id, string newHashedPassword);
	Task<EmployeeEntity> GetUserByRefreshTokenAsync(string refreshToken);
	Task<string> GetEmployeePasswordByIdAsync(int Id);
}



