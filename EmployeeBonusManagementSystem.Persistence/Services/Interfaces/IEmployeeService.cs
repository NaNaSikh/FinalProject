using System.Collections.Generic;
using System.Threading.Tasks;

using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.AddEmployee;

namespace EmployeeBonusManagement.Application.Services.Interfaces
{
	public interface IEmployeeService<T>
	{
		//Task AddEmployeeAsync(EmployeeDto employeeDto, string role);
		Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
		//Task<EmployeeDto> GetEmployeeByIdAsync(string id);
	}
}