using EmployeeBonusManagementSystem.Domain.Entities;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace EmployeeBonusManagementSystem.Application.Contracts.Persistence;

public interface IEmployeeRepository
{
	Task<bool> ExistsByPersonalNumberAsync(string personalNumber);
	Task<bool> ExistsByEmailAsync(string email);
	Task AddEmployeeAsync(EmployeeEntity employee, string role , IDbTransaction transaction);
	Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync();
	Task<EmployeeEntity> GetByEmailAsync(string email);
	Task<bool> CheckPasswordAsync(EmployeeEntity user, string enteredPassword);
	Task<List<string>> GetUserRolesAsync(int employeeId);
	Task<IEnumerable<EmployeeEntity>> GetEmployeeSalary(int Id);
	Task<IEnumerable<BonusEntity>> GetEmployeeBonus(int Id);
	Task<IEnumerable<EmployeeEntity>> GetEmployeeRecomender(int Id);

    Task<(bool, int)> GetEmployeeExistsByPersonalNumberAsync(string personalNumber);
    Task<PasswordVerificationResult> CheckPasswordByIdAsync(int id, string enteredPassword);
	Task UpdateEmployeePasswordByIdAsync(int Id, string newHashedPassword);
}



